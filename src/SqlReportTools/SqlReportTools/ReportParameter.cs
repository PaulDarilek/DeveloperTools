using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace SqlReportTools
{
    /*
        <ReportParameters> 
            <ReportParameter Name="EmployeeName">
                <DataType>String</DataType>
                <DefaultValue><Values><Value/></Values></DefaultValue>
                <AllowBlank>true</AllowBlank>
                <Prompt>Employee Name</Prompt>
                <UsedInQuery>True</UsedInQuery>
            </ReportParameter>
        </ReportParameters>

        <Query>
            <QueryParameters>
              <QueryParameter Name="@EmployeeName">
                <Value>=Parameters!EmployeeName.Value</Value>
              </QueryParameter>
            </QueryParameters>
        </Query>
     */

    /// <summary>ReportParameters from SSRS *.rdl file</summary>
    /// <remarks>These are at the report level and connect to <see cref="ReportDataSet.QueryParameters"/> </remarks>
    public class ReportParameter
    {
        /// <summary>Name of Parameter (xpath = /ReportParameters/ReportParameter[Name])</summary>
        public string Name { get; set; }

        /// <summary>Value of the Parameter after input</summary>
        public List<string> Values { get; }   = new List<string>();

        /// <summary>name like "String" (xpath = /ReportParameters/ReportParameter[Name]/DataType)</summary>
        public string DataType { get; set; }

        /// <summary>name like "String" (xpath = /ReportParameters/ReportParameter[Name]/DefaultValue/Values/Value)</summary>
        public List<string> DefaultValues { get; } = new List<string>();

        /// <summary>name like "String" (xpath = /ReportParameters/ReportParameter[Name]/DefaultValue/Values/Value)</summary>
        /// <remarks>key is the label, value is the value for stored procedure</remarks>
        public HashSet<ValidValue> ValidValues { get; } = new HashSet<ValidValue>();

        /// <summary>(xpath = /ReportParameters/ReportParameter[Name]/AllowBlank)</summary>
        public bool? AllowBlank { get; set; }

        /// <summary>(xpath = /ReportParameters/ReportParameter[Name]/Prompt)</summary>
        public string Prompt { get; set; }

        /// <summary>(xpath = /ReportParameters/ReportParameter[Name]/UsedInQuery)</summary>
        public bool? UsedInQuery { get; set; }

        /// <summary>(xpath = /ReportParameters/ReportParameter[Name]/MultiValue)</summary>
        public bool? MultiValue { get; set; }

        public string GetValue()
        {
            var values = Values.Count > 0 ? Values : DefaultValues;
            return
                (values.Count == 0) ? null :
                (values.Count == 1) ? values[0] :
                string.Join(",", values);
        } 

    }

    [DebuggerStepThrough]
    public struct ValidValue
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

}
