using System.Collections.Generic;

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
            <DataSourceName>MyEmployees</DataSourceName>
            <QueryParameters>
              <QueryParameter Name="@EmployeeName">
                <Value>=Parameters!EmployeeName.Value</Value>
              </QueryParameter>
            </QueryParameters>
            <CommandType>StoredProcedure</CommandType>
            <CommandText>rptEmployees</CommandText>
        </Query>
     */
    public class Parameter
    {

        /// <summary>Name of Parameter (xpath = /ReportParameters/ReportParameter[Name])</summary>
        public string Name { get; set; }

        /// <summary>Stored Proc Parameter Name (xpath = /Query/QueryParameters/QueryParameter[Name])</summary>
        public string ParameterName { get; set; }

        /// <summary>name like "String" (xpath = /ReportParameters/ReportParameter[Name]/DataType)</summary>
        public string DataType { get; set; }

        /// <summary>name like "String" (xpath = /ReportParameters/ReportParameter[Name]/DefaultValue/Values/Value)</summary>
        public List<string> DefaultValue { get; set; }

        /// <summary>(xpath = /ReportParameters/ReportParameter[Name]/AllowBlank)</summary>
        public bool? AllowBlank { get; set; }

        /// <summary>(xpath = /ReportParameters/ReportParameter[Name]/Prompt)</summary>
        public string Prompt { get; set; }

        /// <summary>(xpath = /ReportParameters/ReportParameter[Name]/UsedInQuery)</summary>
        public bool? UsedInQuery { get; set; }
    }
}
