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
        public FileInfo File { get; }

        /// <summary>Unique Guid type Identifier</summary>
        public string ReportID { get; set; }

        /// <summary>Optional Parameters for the Report</summary>
        public HashSet<ReportParameter> Parameters { get; } = new HashSet<ReportParameter>();

        /// <summary>Data Sources</summary>
        public HashSet<ReportDataSource> DataSources { get; } = new HashSet<ReportDataSource>();

        /// <summary>DataSets that utilize one ore more DataSources</summary>
        public HashSet<ReportDataSet> DataSets { get; } = new HashSet<ReportDataSet>();


        /// <summary>Constructor</summary>
        /// <param name="file">Location of .rdl or .rdlc file</param>
        /// <exception cref="ArgumentNullException"><paramref name="file"/> must not be null</exception>
        /// <exception cref="FileNotFoundException"><paramref name="file"/> must exist</exception>
        public ReportDefinition(FileInfo file)
        {
            File = file ?? throw new ArgumentNullException("file");
            if (!File.Exists) throw new FileNotFoundException(File.FullName);
        }

        /// <summary>Connect</summary>
        /// <param name="sources"></param>
        internal void AddDataSources(IEnumerable<ReportDataSource> sources) => DataSources.UnionWith(sources);

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
                foreach(var qParm in rptDataSet.QueryParameters)
                {
                    var name = qParm.Name;
                    var value = qParm.Value;
                    if(value == null && qParm.Parameter != null)
                    {
                        var list = qParm.Parameter.Values.Count > 0 ? qParm.Parameter.Values : qParm.Parameter.DefaultValues;
                        value =
                            list.Count == 0 ? (qParm.Parameter.AllowBlank == false  ? string.Empty: null) :
                            list.Count == 1 ? list[0] :
                            string.Join(",", list);
                    }
                    cmd.Parameters.AddWithValue(name, value);
                }

                var dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dataSet, rptDataSet.Name);
                logMessage?.Invoke($"Done: {rptDataSet.DataSourceName}");
            }

            return dataSet;
        }


    }
}
