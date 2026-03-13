using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace SqlReportTools.WinForms
{
    public partial class ViewReport : Form
    {
        private Settings Settings { get; }
        private ReportRunner Helper { get; }
        private bool IgnoreCombo { get; set;  }

        public ViewReport(Settings settings)
        {
            Settings = settings;
            Helper = new ReportRunner(settings, AppendMessage);

            InitializeComponent();

            CenterToScreen();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Helper.ScanFiles();

            try
            {
                IgnoreCombo = true;
                FillCombos();
            }
            finally
            {
                IgnoreCombo = false;
            }

            TxtInputFolder.Text = Settings.SqlReportsDirectory.FullName;
            TxtOutputFolder.Text = Settings.OutputDirectory?.FullName;
            TxtSqlServer.Text = Settings.SqlServer;
        }

        private void FillCombos()
        {
            ReportCombo.Items.AddRange(Helper.RdlManager.GetReportPaths());

            ReportCombo.SelectedIndex = ReportCombo.Items.Count > 0 ? 0 : -1;

            foreach (var item in Helper.RdsManager.GetDataSources())
            {
                ComboDataSource.Items.Add($"{item.ConnectString} [{item.DataSourceID}]");
            }
            ComboDataSource.SelectedIndex = ComboDataSource.Items.Count > 0 ? 0 : -1;
        }

        private void BtnOpenReport_Click(object sender, EventArgs e)
        {
            ClearMessage();
            var report = Helper.GetReportDefinition(new FileInfo(ReportCombo.Text));
            if (report != null)
            {
                var success = Helper.Run(ReportViewer, report);
                Tabs.SelectedIndex = success ? 1 : 0;
            }
        }

        private void AppendMessage(string message) => this.TxtReport.Text += Environment.NewLine + message;
        
        private void ClearMessage() => this.TxtReport.Text = string.Empty;

        private void BtnDataSource_Click(object sender, EventArgs e)
        {
            ClearMessage();

            var text = ComboDataSource.Text;
            var posStart = text.LastIndexOf('[');
            var posEnd = text.IndexOf(']', Math.Max(0, posStart));
            if (posStart >= 0 && posEnd >= posStart)
            {
                var id = text.Substring(posStart+1, posEnd - posStart - 1);
                var usage =
                    Helper.RdlManager.GetReports(x => x.DataSources.Any(ds => ds.DataSourceID == id))
                    .ToList();
                foreach (var report in usage)
                {

                    AppendMessage(report.File.FullName);
                }
            }
        }

        private void ReportCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IgnoreCombo) return;
            ReportViewer.Reset();
            Tabs.SelectedIndex = 0;
            ClearMessage();
            var report = Helper.GetReportDefinition(new FileInfo(ReportCombo.Text));
            if (report != null)
            {
                var json = JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
                AppendMessage(json);
                var InputList = report.Parameters.Select(x => new InputParameter(x)).ToList();
                if (InputList.Count > 0)
                {
                    GridView.DataSource = InputList;

                    if (Tabs.SelectedIndex != 2)
                    {
                        Tabs.SelectedIndex = 2;
                    }
                }
                else
                {
                    GridView.DataSource = null;
                    if (Tabs.SelectedIndex != 0)
                    {
                        Tabs.SelectedIndex = 0;
                    }
                }

                ReportCombo.Focus();
            }
        }

        private void ComboDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IgnoreCombo) return;
            Tabs.SelectedIndex = 0;
            var text = ComboDataSource.Text;
            var posStart = text.LastIndexOf('[');
            var posEnd = text.IndexOf(']', Math.Max(0, posStart));
            if (posStart >= 0 && posEnd >= posStart)
            {
                ClearMessage();
                var id = text.Substring(posStart + 1, posEnd - posStart - 1);
                var ds = Helper.RdsManager.GetDataSourceById(id);
                if (ds != null)
                {
                    var json = JsonSerializer.Serialize(ds, new JsonSerializerOptions { WriteIndented = true });
                    AppendMessage(json);
                }
            }
        }

        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            Settings.SqlServer = string.IsNullOrEmpty(TxtSqlServer.Text) ? null : TxtSqlServer.Text;

            if (! string.IsNullOrEmpty(TxtInputFolder.Text))
                Settings.SqlReportsDirectory = new DirectoryInfo(TxtInputFolder.Text);

            Settings.OutputDirectory = 
                string.IsNullOrEmpty(TxtOutputFolder.Text) ? 
                null : 
                new DirectoryInfo(TxtOutputFolder.Text);

            Helper.ScanFiles();
            Tabs.SelectedIndex = 0;
        }
    }
}
