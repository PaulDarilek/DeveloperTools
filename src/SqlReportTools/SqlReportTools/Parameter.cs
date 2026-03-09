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
            <DataSourceName>ERIS</DataSourceName>
            <QueryParameters>
              <QueryParameter Name="@EmployeeName">
                <Value>=Parameters!EmployeeName.Value</Value>
              </QueryParameter>
            </QueryParameters>
            <CommandType>StoredProcedure</CommandType>
            <CommandText>rpt4021DEG9DbN</CommandText>
        </Query>
     */
    public class Parameter
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public List<string> DefaultValue { get; set; }
        public bool? AllowBlank { get; set; }
        public string Prompt { get; set; }
        public bool? UsedInQuery { get; set; }
    }
}
