using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace SqlReportTools
{
    [DebuggerStepThrough]
    /// <summary>Report Definition Language (rdl file)</summary>
    public class ReportDefinition
    {
        /// <summary>File containing the XML definition</summary>
        [JsonConverter(typeof(FileInfoJsonConverter))]
        public FileInfo File { get; set; }

        public string ReportID { get; set; }

        /// <summary>Data Sources</summary>
        public HashSet<ReportDataSource> DataSources { get; } = new HashSet<ReportDataSource>();

        public HashSet<ReportDataSet> DataSets { get; } = new HashSet<ReportDataSet>();

        public List<Parameter> Parameters { get; } = new List<Parameter>();
        
        public ReportDefinition(FileInfo file)
        {
            File = file ?? throw new ArgumentNullException("file");
        }

        public void AddDataSources(IEnumerable<ReportDataSource> sources) => DataSources.UnionWith(sources);

        //TODO: Add Parameters!
        public DataSet FillDataSets(Action<string> logMessage, string devSqlServer = null)
        {
            var dataSet = new DataSet();

            foreach (var rptDataSet in DataSets)
            {
                logMessage?.Invoke($"Connect DataSet: {rptDataSet.Name}");

                var dataSource =
                    DataSources.FirstOrDefault(x => x.Name == rptDataSet.DataSourceName) ??
                    DataSources.FirstOrDefault(x => x.Name == rptDataSet.Name);

                logMessage?.Invoke($"Connecting: {dataSource.ConnectString}");
                var conn = dataSource.OpenSqlConnection(devSqlServer);
                logMessage?.Invoke($"Connected: {dataSource.Name}");

                logMessage?.Invoke($"Fill: {rptDataSet.DataSourceName}");
                var cmd = new SqlCommand()
                {
                    CommandText = rptDataSet.CommandText,
                    CommandType = rptDataSet.CommandType == "StoredProcedure" ? CommandType.StoredProcedure : CommandType.Text,
                    Connection = conn,
                    CommandTimeout = 120,
                };
                //TODO: Add Parameters!
                //cmd.Parameters.Add()

                var dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dataSet, rptDataSet.Name);
                logMessage?.Invoke($"Done: {rptDataSet.DataSourceName}");
            }

            return dataSet;
        }


    }
}
