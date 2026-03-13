using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SqlReportTools
{
    /// <summary>Collects and Manages SSRS reports (*.rdl) in a Directory</summary>
    public class ReportManager
    {

        private HashSet<ReportDefinition> Reports { get; }
        private Action<string> Logger { get; }

        /// <summary>Constructor</summary>
        public ReportManager(Action<string> logger) {
            Logger = logger;
            Reports = new HashSet<ReportDefinition>();
        }

        public string[] GetReportPaths() => Reports.Select(x => x.File?.FullName).ToArray();
        
        public IEnumerable<ReportDefinition> GetReports(Func<ReportDefinition, bool> predicate) => predicate == null ? Reports : Reports.Where(predicate);


        /// <summary>Load Files from the File System</summary>
        /// <param name="directory"></param>
        /// <param name="options"></param>
        public IEnumerable<ReportDefinition> AddReports(DataSourceManager dataSourceManager, DirectoryInfo directory, SearchOption options = SearchOption.AllDirectories)
        {
            var reports = new List<ReportDefinition>();
            var allFiles = directory.EnumerateFiles("*.rdl", options).ToList();
            allFiles.AddRange(directory.EnumerateFiles("*.rdlc", options));
            foreach (var file in allFiles)
            {
                var report = AddReport(file, dataSourceManager);
                reports.Add(report);
            }
            return reports;
        }

        public ReportDefinition AddReport(FileInfo file, DataSourceManager dataSourceManager)
        {
            ReportDefinition report = new ReportDefinition(file);
            ParseXml(report, dataSourceManager);
            var crLfTab = Environment.NewLine + '\t';
            Log($"{report.File.FullName} {crLfTab}{string.Join(crLfTab, report.DataSets.Select(Format))}");
            AddReport(report);
            return report;

            string Format(ReportDataSet x) => $"[{x.Name}] = {x.CommandText}";
        }

        /// <summary>Reads the Report RDL File and parses the XML</summary>
        /// <param name="report"></param>
        /// <param name="dataSourceManager"></param>
        /// <returns></returns>
        public bool ParseXml(ReportDefinition report, DataSourceManager dataSourceManager)
        {
            if (report.File == null || !report.File.Exists) return false;

            string xml = File.ReadAllText(report.File.FullName);

            XElement doc = XElement.Parse(xml);
            XNamespace ns = doc.GetDefaultNamespace();

            report.ReportID = doc.Elements().FirstOrDefault(x => x.Name.LocalName == "ReportID")?.Value;

            ParseReportParameters(report, doc, ns);

            if (dataSourceManager != null)
            {
                var dataSources = dataSourceManager.ParseRdlDataSources(doc).ToList();
                report.AddDataSources(dataSources);
            }


            XElement dataSets = doc.Elements().FirstOrDefault(x => x.Name.LocalName == "DataSets");
            foreach (XElement node in dataSets.Elements(ns + "DataSet"))
            {
                var dataSet = ParseXmlDataSet(node, ns, report.Parameters);
                report.DataSets.Add(dataSet);
            }
            return true;
        }

        private void AddReport(ReportDefinition report)
        {
            Debug.Assert(report?.File?.FullName != null);
            var match = Reports.FirstOrDefault(x => x.File.FullName == report.File.FullName);
            if (match != null)
            {
                Reports.Remove(match);
            }
            Reports.Add(report);
        }

        private void ParseReportParameters(ReportDefinition report, XElement doc, XNamespace ns)
        {
            var xParms = 
                doc.Element(ns + "ReportParameters") ??
                doc.Elements().FirstOrDefault(x => x.Name.LocalName == "ReportParameters");
            
            if (xParms != null)
            {
                foreach (var xParm in xParms.Elements(ns + "ReportParameter"))
                {
                    var rptParm = new ReportParameter
                    {
                        Name = xParm.Attribute("Name")?.Value,
                        DataType = xParm.Element(ns + "DataType")?.Value,
                        AllowBlank = xParm.Element(ns + "AllowBlank")?.Value.Equals("true", StringComparison.OrdinalIgnoreCase),
                        Prompt = xParm.Element(ns + "Prompt")?.Value,
                        UsedInQuery = xParm.Element(ns + "UsedInQuery")?.Value.Equals("True", StringComparison.OrdinalIgnoreCase),
                        MultiValue = xParm.Element(ns + "MultiValue")?.Value.Equals("true", StringComparison.OrdinalIgnoreCase),
                    };

                    XElement values = xParm.Element(ns + "DefaultValue")?.Element(ns + "Values");
                    if (values != null)
                    {
                        foreach (var value in values.Elements(ns + "Value"))
                        {
                            var strValue = ParseValue(value?.Value);
                            if(strValue != null)
                            {
                                rptParm.DefaultValues.Add(strValue);
                            }
                        }
                    }

                    XElement validValues = xParm.Element(ns + "ValidValues");
                    XElement parameterValues = validValues?.Element(ns + "ParameterValues");

                    if (validValues != null && parameterValues != null)
                    {
                        foreach (var parmValue in parameterValues.Elements(ns + "ParameterValue"))
                        {
                            var pValue = new ValidValue
                            {
                                Value = parmValue.Element(ns + "Value")?.Value,
                                Label = parmValue.Element(ns + "Label")?.Value
                            };
                            if (pValue.Value != null)
                            {
                                if(pValue.Value.StartsWith("=") == true)
                                {
                                    pValue.Label = pValue.Label ?? pValue.Value;
                                    pValue.Value = ParseValue(pValue.Value);
                                }
                                rptParm.ValidValues.Add(pValue);
                            }
                        }
                    }

                    report.Parameters.Add(rptParm);
                }
            }
        }

        private string ParseValue(string value)
        {
            if (string.IsNullOrEmpty(value) || value[0] != '=')
                return value;

            switch (value)
            {
                case "=User!UserID": 
                    return $"{Environment.UserDomainName}\\{Environment.UserName}";
                default:
                    FixSpacing("=()&"); // fix spacing around these characters
                    ReplaceValue($"\"{DateTime.Today.Year}\"", "Year(Today)");
                    ReplaceValue("", "\"&\""); // combine concatenated strings together
                    if(value.StartsWith("=CDate(\"") && value.EndsWith("\")"))
                    {
                        value = value.Substring(8, value.Length - 10);
                        return value;
                    }
                    Log($"TODO: Parse {value}");

                    break;
            }

            return value;

            void FixSpacing(string chars)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    char ch = chars[i];
                    ReplaceValue($"{ch}", $" {ch} ", $"{ch} ", $" {ch}");
                }
            }

            void ReplaceValue(string replacement, params string[] keys)
            {
                foreach (var key in keys)
                {
                    if (value.Contains(key))
                    {
                        value = value.Replace(key, replacement);
                    }
                }
            }
        }


        private ReportDataSet ParseXmlDataSet(XElement dataSetNode, XNamespace ns, IEnumerable<ReportParameter> reportParameters)
        {
            var dataSet = new ReportDataSet
            {
                Name = dataSetNode.Attribute("Name")?.Value
            };

            var query = dataSetNode.Element(ns + "Query");
            Debug.Assert(query != null);
            if (query != null)
            {
                dataSet.DataSourceName = query.Element(ns + "DataSourceName")?.Value;
                dataSet.CommandType = query.Element(ns + "CommandType")?.Value;
                dataSet.CommandText = query.Element(ns + "CommandText")?.Value;

                ParseQueryParms(dataSet, query, ns, reportParameters);
            }

            Debug.Assert(dataSet.IsValid);

            var fields = dataSetNode.Element(ns + "Fields");
            Debug.Assert(fields != null);
            foreach (XElement fieldNode in fields.Elements(ns + "Field"))
            {
                var field = new ReportField()
                {
                    Name = fieldNode.Attribute("Name").Value,
                    DataField = fieldNode.Element(ns + "DataField")?.Value,
                    TypeName = fieldNode.Elements().FirstOrDefault(x => x.Name.LocalName == "TypeName")?.Value,
                };
                dataSet.Fields.Add(field);
            }
            Debug.Assert(dataSet.Fields.Count > 0);
            return dataSet;
        }

        private static void ParseQueryParms(ReportDataSet dataSet, XElement query, XNamespace ns, IEnumerable<ReportParameter> reportParameters)
        {
            var parms = query.Element(ns + "QueryParameters");
            if (parms != null)
            {
                foreach (var item in parms.Elements(ns + "QueryParameter"))
                {
                    var qParm = new QueryParameter()
                    {
                        Name = item.Attribute("Name")?.Value,
                        Value = item.Element(ns + "Value").Value,
                        Parameter = null,
                    };
                    qParm.SetParameterReference(reportParameters);
                    dataSet.QueryParameters.Add(qParm);
                }
            }
        }

        private void Log(string message)
        {
            Logger?.Invoke(message);
            Trace.WriteLine(message);
        }

    }
}
