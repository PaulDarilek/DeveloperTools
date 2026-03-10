using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace SqlReportTools.WinForms
{
    public partial class ViewReport : Form
    {
        public ReportRunner Helper { get; }
        private bool IgnoreCombo { get; set;  }

        public ViewReport(Settings settings)
        {

            Helper = new ReportRunner(settings, AppendMessage);
            InitializeComponent();

            Text = "View Report Form";
            //ClientSize = new Size(640, 320);
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
            var report = Helper.LocateReport(new FileInfo(ReportCombo.Text));
            if (report != null)
            {
                Helper.Run(this.ReportViewer, report);
                TabReports.Focus();
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
            ClearMessage();
            var report = Helper.LocateReport(new FileInfo(ReportCombo.Text));
            if (report != null)
            {
                var json = JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
                AppendMessage(json);
            }
        }

        private void ComboDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IgnoreCombo) return;
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
    }
}
