using System.Collections.Generic;
using System.Diagnostics;

namespace SqlReportTools
{
    /// <summary>DataSet within a SSRS Report (*.rdl file)</summary>
    public class ReportDataSet
    {
        /// <summary>Name of the DataSet (from the rdl file)</summary>
        /// <remarks>xpath: Report/DataSets/DataSet[Name]</remarks>
        public string Name { get; set; }

        /// <summary>Name of the DataSource (from the rdl file)</summary>
        /// <remarks>xpath: Report/DataSets/DataSet/Query/DataSourceName</remarks>
        public string DataSourceName { get; set; }

        /// <summary>Command Type (StoredProcedure or null?)</summary>
        /// <remarks>xpath: Report/DataSets/DataSet/Query/CommandType</remarks>
        public string CommandType { get; set; }

        /// <summary>SQL Stored Procedure Name or Sql Statement</summary>
        /// <remarks>xpath: Report/DataSets/DataSet/Query/CommandText</remarks>
        public string CommandText { get; set; }

        /// <summary>Optional Parameters used to create the data</summary>
        public List<QueryParameter> QueryParameters { get; }


        /// <summary>Names of Fields (ignore DataField and TypeName)</summary>
        /// <remarks>xpath: Report/DataSets/DataSet/Fields</remarks>
        public List<ReportField> Fields { get; } 

        public bool IsValid =>
            !string.IsNullOrEmpty(Name) &&
            !string.IsNullOrEmpty(DataSourceName) &&
            !string.IsNullOrEmpty(CommandText);

        [DebuggerStepThrough]
        public ReportDataSet()
        {
            Fields = new List<ReportField>();
            QueryParameters = new List<QueryParameter>();
        }

    }
}
