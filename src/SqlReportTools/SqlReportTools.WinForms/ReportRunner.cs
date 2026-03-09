using Microsoft.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace SqlReportTools.WinForms
{
    public class ReportRunner
    {
        private Settings Settings { get; }

        public ReportManager RdlManager { get; } 
        public DataSourceManager RdsManager { get; } 

        public FileInfo ReportFile { get; private set; }
        public string ReportXml { get; private set; }
        public ReportDefinition Report { get; private set; }
        private LocalReport LocalReport { get; set; }
        private Action<string> SendMessage { get; set;  }

        public ReportRunner(Settings settings)
        {
            Settings = settings;
 
            RdlManager = new ReportManager();
            RdsManager = new DataSourceManager();
        }

        public void ScanFiles()
        {
            RdsManager.ScanRdsFiles(Settings.SqlReportsDirectory);
            RdlManager.AddReports(RdsManager, Settings.SqlReportsDirectory);
        }

        public bool LocateReport(FileInfo reportFile, Action<string> sendMessage)
        {
            ReportFile = reportFile;
            SendMessage = sendMessage;

            ReportXml = null;
            Report = null;

            if (!reportFile.Exists)
            {
                SendMessage?.Invoke("File Does not exist.");
                return false;
            }

            ReportXml = File.ReadAllText(ReportFile.FullName);

            Report =
                RdlManager.GetReports().Where(x => x.File.FullName == reportFile.FullName).FirstOrDefault() ??
                RdlManager.AddReport(reportFile, RdsManager);
            if (Report == null)
            {
                sendMessage?.Invoke("Error: Cannot Parse Report!");
                return false;
            }
            
            return true;
        }

        public void Run(ReportViewer viewer)
        {
            if (ReportFile == null || !ReportFile.Exists || Report == null || Report.File.Name != ReportFile.Name)
            {
                return;
            }

            // Set the processing mode for the ReportViewer to Local  
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = ReportFile.FullName;

            LocalReport = viewer.LocalReport;
            AddReportParameter();

            Report.FillDataSets(SendMessage);
            // -------------------------
            // Data Sets
            // -------------------------
            foreach (var rptDataSet in Report.DataSets )
            {
                var dataSet = BuildDataSet(rptDataSet);
                SendMessage?.Invoke($"Connect DataSet: {rptDataSet.Name}");
                AddDataSetToLocalReport(dataSet);
            }

            AddDataSources();

            // Refresh the report  
            viewer.RefreshReport();
        }

        private void AddReportParameter()
        {
            foreach (var parm in LocalReport.GetParameters())
            {
                Trace.WriteLine($"{parm.Name} = {parm.DataType}");
            }

            //// Create a report parameter for the sales order number   
            //ReportParameter rpSalesOrderNumber = new ReportParameter
            //{
            //    Name = "SalesOrderNumber",
            //    Values = new StringCollection() { TableName },
            //};

            //// Set the report parameters for the report  
            //localReport.SetParameters(
            //    new ReportParameter[] { rpSalesOrderNumber });
        }

        private DataSet BuildDataSet(ReportDataSet rptDataSet)
        {
            DataSet dataset = new DataSet(rptDataSet.Name);
            var dataSource =
                RdsManager.GetDataSourceByName(rptDataSet.DataSourceName) ??
                RdsManager.GetDataSourceByName(rptDataSet.Name);

            SendMessage?.Invoke($"Connecting: {dataSource.ConnectString}");
            var conn = dataSource.OpenSqlConnection();
            SendMessage?.Invoke($"Connected: {dataSource.Name}");

            SendMessage?.Invoke($"Fill: {rptDataSet.DataSourceName}");
            rptDataSet.FillDataSet(conn, dataset);
            SendMessage?.Invoke($"Done: {rptDataSet.DataSourceName}");

            //Get Each DataAdapter and fill the dataset
            //var tablesAdapter = GetTableAdapter(TableName);
            //tablesAdapter.Fill(dataset, "Tables");

            return dataset;
        }

        private void AddDataSetToLocalReport(DataSet dataset)
        {
            var expected = LocalReport.GetDataSourceNames();
            Debug.Assert(expected.Count > 0);

            foreach (DataTable table in dataset.Tables)
            {
                Microsoft.Reporting.WinForms.ReportDataSource rptDataSource = new Microsoft.Reporting.WinForms.ReportDataSource
                {
                    Name = table.TableName,
                    Value = table,
                };

                LocalReport.DataSources.Add(rptDataSource);
            }
        }

        private void AddDataSources()
        {
            string[] sources = LocalReport.GetDataSourceNames().ToArray();
            Debug.Assert(sources.Length > 0);

            Debug.Assert(LocalReport.DataSources.Count == Report.DataSources.Count);

            foreach (var item in Report.DataSources)
            {
                Debug.WriteLine($"{item.Name} = {item.ConnectString}");
            }
        }

 
    }
}
