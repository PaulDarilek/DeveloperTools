using System.Linq;

namespace SqlReportTools.WinForms
{

    /// <summary>Class for Display and Input of Parameters</summary>
    public class InputParameter
    {
        public string Name { get; }
        
        public bool? AllowBlank { get; }
        
        public bool? MultiValue { get; }

        public string ValidValues { get; }

        public string Prompt { get; }

        public string Value
        {
            get => Parameter.Value;
            set => Parameter.Value = value;
        }

        private ReportParameter Parameter { get; }

        public InputParameter(ReportParameter parameter)
        {
            Parameter = parameter;
            Name = parameter.Name;
            Prompt = parameter.Prompt;
            AllowBlank = parameter.AllowBlank;
            MultiValue = parameter.MultiValue;
            ValidValues = string.Join(",", parameter.ValidValues.Select(x => x.Value));

            Value = parameter.DefaultValues.Count == 0 ? null : string.Join(",", parameter.DefaultValues);
        }

    }
}
