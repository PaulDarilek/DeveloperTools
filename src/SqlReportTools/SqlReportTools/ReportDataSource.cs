using Microsoft.Data.SqlClient;
using System.IO;
using System.Text.Json.Serialization;

namespace SqlReportTools
{

    /// <summary>ReportDataSource (rds file)</summary>
    public class ReportDataSource
    {
        /// <summary>File containing the XML definition</summary>
        [JsonConverter(typeof(FileInfoJsonConverter))]
        public FileInfo File { get; set; }

        /// <summary>xpath: RptDataSource[Name]</summary>
        public string Name { get; set; }

        /// <summary>xpath: RptDataSource\DataSourceID</summary>
        public string DataSourceID { get; set;   }

        /// <summary>xpath: RptDataSource\ConnectionProperties\ConnectString</summary>
        public string ConnectString { get; set; }

        /// <summary>xpath: RptDataSource\ConnectionProperties\IntegratedSecurity</summary>
        public bool? IntegratedSecurity { get; set; }

        /// <summary>xpath: RptDataSource\ConnectionProperties\Extension</summary>
        public string Extension { get; set; }

        public bool IsValid => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(DataSourceID) && !string.IsNullOrEmpty(ConnectString);

        public SqlConnection OpenSqlConnection()
        {
            var builder = new SqlConnectionStringBuilder(ConnectString)
            {
                IntegratedSecurity = true
            };
            ConnectString = builder.ToString();

            var sqlConnection = new SqlConnection(ConnectString);
            sqlConnection.Open();
            return sqlConnection;
        }


    }
}
