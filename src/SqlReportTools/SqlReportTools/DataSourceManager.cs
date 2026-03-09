using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SqlReportTools
{
    /// <summary>Handler for everything related to SSRS Data Sources</summary>
    public class DataSourceManager
    {
        private HashSet<ReportDataSource> DataSources { get; } 
        private const StringComparison IgnoreCase = StringComparison.InvariantCultureIgnoreCase;

        /// <summary>Constructor</summary>
        [DebuggerStepThrough]
        public DataSourceManager()
        {
            DataSources = new HashSet<ReportDataSource>();
        }

        public IEnumerable<ReportDataSource> GetDataSources() => DataSources.ToArray();

        public ReportDataSource GetDataSourceById(string dataSourceID) => DataSources.FirstOrDefault(x => x.DataSourceID.Equals(dataSourceID, IgnoreCase));
        public ReportDataSource GetDataSourceByName(string name) => DataSources.FirstOrDefault(x => x.Name.Equals(name, IgnoreCase));
        public ReportDataSource GetDataSourceByFile(FileInfo file) 
        {
            if (file == null) return null;
            return DataSources.FirstOrDefault(x => x.File != null && x.File.FullName.Equals(file.FullName, IgnoreCase));
        }

        /// <summary>Load Files from the File System</summary>
        /// <param name="directory"></param>
        /// <param name="options"></param>
        public void ScanRdsFiles(DirectoryInfo directory, SearchOption options = SearchOption.AllDirectories)
        {
            var allFiles = directory.EnumerateFiles("*.rds", options);
            foreach (var file in allFiles)
            {
                AddFromFile(file);
            }
        }

        public ReportDataSource AddFromFile(FileInfo file)
        {
            ReportDataSource dataSource = new ReportDataSource() { File = file };
            if(ParseFile(dataSource))
            {
                FindOrAdd(dataSource);
            }
            else
            {
                Debug.Assert(!string.IsNullOrEmpty(dataSource.ConnectString));
            }
            return dataSource;
        }


        public ReportDataSource FindOrAdd(ReportDataSource dataSource)
        {
            ReportDataSource match =
                GetDataSourceByFile(dataSource.File)
                ?? GetDataSourceById(dataSource.DataSourceID)
                ?? GetDataSourceByName(dataSource.Name);

            if (match == null)
            {
                // not found... add it.
                match = dataSource;
                DataSources.Add(dataSource);
            }

            return match;
        }

        /// <summary>Parse RDL file</summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public bool ParseFile(ReportDataSource dataSource)
        {
            if (dataSource.File == null || !dataSource.File.Exists) return false;

            string xml = File.ReadAllText(dataSource.File.FullName);
            
            XElement doc = XElement.Parse(xml);

            Debug.Assert(doc.Name.LocalName == "RptDataSource");

            dataSource.Name =  doc.Attribute("Name")?.Value ?? doc.Element("Name")?.Value;
            
            if (string.IsNullOrEmpty(dataSource.Name))
            {
                Debug.Assert(!string.IsNullOrEmpty(dataSource.Name));
                dataSource.Name = Path.GetFileNameWithoutExtension(dataSource.File.Name);
            }

            dataSource.DataSourceID = doc.Element("DataSourceID")?.Value ?? string.Empty;

            XElement connectionProperties = doc.Element("ConnectionProperties");
            Debug.Assert(connectionProperties != null);

            if (connectionProperties != null)
            {
                dataSource.ConnectString = connectionProperties.Element("ConnectString")?.Value ?? string.Empty;

                var integratedSecurity = connectionProperties.Element("IntegratedSecurity")?.Value;
                if (integratedSecurity != null && bool.TryParse(integratedSecurity, out var parsed))
                {
                    dataSource.IntegratedSecurity = parsed;
                }

                dataSource.Extension = connectionProperties.Element("Extension")?.Value ?? string.Empty;
            }

            Debug.Assert(dataSource.IsValid);
            return dataSource.IsValid;
        }

        /// <summary>Parse from xPath DataSources/DataSource stored in SSRS *.rdl files</summary>
        public IEnumerable<ReportDataSource> ParseDataSources(XElement doc)
        {
            XElement root = doc.Elements().FirstOrDefault(x => x.Name.LocalName == "DataSources");
            Debug.Assert(root != null && root.Name.LocalName == "DataSources");

            if (root != null)
            {
                XNamespace ns = root?.GetDefaultNamespace();
                foreach (XElement node in root.Elements(ns + "DataSource"))
                {
                    var dataSource = new ReportDataSource
                    {
                        Name = node.Attribute("Name").Value,
                        DataSourceID = node.Elements().FirstOrDefault(x => x.Name.LocalName == "DataSourceID")?.Value ?? string.Empty
                    };

                    // Connect to Global List.
                    dataSource = FindOrAdd(dataSource);
                    yield return dataSource;
                }
            }
        }

    }
}
