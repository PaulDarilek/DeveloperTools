using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SqlReportTools
{
    /*
        <Report>
            <Query>
                <QueryParameters>
                    <QueryParameter Name="@EmployeeName">
                        <Value>=Parameters!EmployeeName.Value</Value>
                    </QueryParameter>
                </QueryParameters>
            </Query>
        </Report>
    */

    public class QueryParameter
    {
        /// <summary><see cref="ReportParameter.Name"/> (from xPath: Report/DataSets/DataSet/Query/QueryParameters/QueryParameter[Name])</summary>
        /// <remarks>Typically will Match the <see cref="Value"/> but without the leading '@' symbol.</remarks>
        public string Name { get; set; }

        /// <summary>Stored Proc Parameter Name (xpath = /Query/QueryParameters/QueryParameter[Name]/Value)</summary>
        /// <remarks>Typically will be prefixed with an @ sign like a SQL parameter name, but may contain a formula</remarks>
        public string Value { get; set; }

        /// <summary>Used for Display</summary>
        [Obsolete("Used only for JSON serialization as quick display")]
        public string ParameterValue => (!string.IsNullOrEmpty(Value) && Value[0] != '=') ? Value : Parameter?.GetValue();

        /// <summary>Reference to the Report Parameter</summary>
        [JsonIgnore]
        public ReportParameter Parameter { get; set;}

        public bool SetParameterReference(IEnumerable<ReportParameter> reportParameters)
        {
            const string prefix = "=Parameters!";
            const string suffix = ".Value";
            var name = Value ?? string.Empty;

            if (name.StartsWith(prefix) && name.EndsWith(suffix))
            {
                name = name.Substring(prefix.Length, name.Length - prefix.Length - suffix.Length );
                Parameter = reportParameters.Where(x => x.Name == name).FirstOrDefault();
            }
            return Parameter != null;
        }

    }
}
