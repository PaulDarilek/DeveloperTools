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

        /// <summary>Constructor</summary>
        public ReportManager() {
            Reports = new HashSet<ReportDefinition>();
        }

        public IEnumerable<ReportDefinition> GetReports() => Reports.ToArray();
        public IEnumerable<ReportDefinition> GetReportsWithDataSource(Func<ReportDataSource, bool> predicate) => Reports.Where(rpt => rpt.DataSources.Any(predicate));


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
            ParseFile(report, dataSourceManager);
            AddReport(report);
            return report;
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

        public bool ParseFile(ReportDefinition report, DataSourceManager dataSourceManager)
        {
            if (report.File == null || !report.File.Exists) return false;

            string xml = File.ReadAllText(report.File.FullName);
            XElement doc = XElement.Parse(xml);

            report.ReportID = doc.Elements().FirstOrDefault(x => x.Name.LocalName == "ReportID")?.Value;

            if(dataSourceManager != null)
            {
                var dataSources = dataSourceManager.ParseDataSources(doc).ToList();
                report.AddDataSources(dataSources);
            }

            XElement root = doc.Elements().FirstOrDefault(x => x.Name.LocalName == "DataSets");
            XNamespace ns = root?.GetDefaultNamespace();
            foreach (XElement node in root.Elements(ns + "DataSet"))
            {
                var dataSet = new ReportDataSet();
                FillFromXml(dataSet, node, ns);
                report.DataSets.Add(dataSet);
            }
            return true;
        }

        private void FillFromXml(ReportDataSet dataSet, XElement node, XNamespace ns)
        {
            dataSet.Name = node.Attribute("Name")?.Value;

            var query = node.Element(ns + "Query");
            Debug.Assert(query != null);
            if (query != null)
            {
                dataSet.DataSourceName = query.Element(ns + "DataSourceName")?.Value;
                dataSet.CommandType = query.Element(ns + "CommandType")?.Value;
                dataSet.CommandText = query.Element(ns + "CommandText")?.Value;
            }
            Debug.Assert(dataSet.IsValid);

            var fields = node.Element(ns + "Fields");
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
        }

    }
}
