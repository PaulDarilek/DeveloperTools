namespace SqlReportTools
{
    public class ReportField
    {
        /// <summary>xpath: Report\DataSets\DataSet[Name]\Fields\Field[Name]</summary>
        public string Name { get; set; }
        /// <summary>xpath: Report\DataSets\DataSet[Name]\Fields\Field[Name]\DataField</summary>
        public string DataField { get; set; }
        /// <summary>xpath: Report\DataSets\DataSet[Name]\Fields\Field[Name]\TypeName</summary>
        public string TypeName { get; set; }
    }
}
