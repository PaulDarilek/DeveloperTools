using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SqlReportTools.WinForms
{
    public class ReportRunner
    {
        private Settings Settings { get; }
        private Action<string> SendMessage { get; }

        public ReportManager RdlManager { get; } 
        public DataSourceManager RdsManager { get; } 


        public ReportRunner(Settings settings, Action<string> sendMessage)
        {
            Settings = settings;
            SendMessage = sendMessage;
 
            RdlManager = new ReportManager();
            RdsManager = new DataSourceManager();
        }

        public void ScanFiles()
        {
            RdsManager.ScanRdsFiles(Settings.SqlReportsDirectory);
            RdlManager.AddReports(RdsManager, Settings.SqlReportsDirectory);
        }

        public ReportDefinition LocateReport(FileInfo reportFile)
        {

            if (!reportFile.Exists)
            {
                Log("File Does not exist.");
                return null;
            }

            var report =
                RdlManager.GetReports(x => x.File.FullName == reportFile.FullName).FirstOrDefault() ??
                RdlManager.AddReport(reportFile, RdsManager);

            if (report == null)
            {
                Log("Error: Cannot Parse Report!");
            }
            
            return report;
        }

        public void Run(ReportViewer viewer, ReportDefinition report)
        {
            if (report == null || report.File == null || !report.File.Exists)
            {
                return;
            }

            // Set the processing mode for the ReportViewer to Local  
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = report.File.FullName;

            var localReport = viewer.LocalReport;
            AddReportParameter(localReport);

            // -------------------------
            // Data Sets
            // -------------------------
            try
            {
                var dataSet = report.FillDataSets(Log, Settings.DevSqlServer);
                AddDataSetToLocalReport(localReport, dataSet);

                AddDataSources(localReport, report);
            }
            catch(Exception ex)
            {
                Log($"Exception: {ex.GetType().Name} {ex.Message}");
            }

            // Refresh the report  
            viewer.RefreshReport();
        }

        /// <summary>Add Parameters to the Report</summary>
        private void AddReportParameter(LocalReport localReport)
        {
            foreach (var parm in localReport.GetParameters())
            {
                Log($"Parameter: {parm.Name} = {parm.DataType}");
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


        private void AddDataSetToLocalReport(LocalReport localReport, DataSet dataset)
        {
            var expected = localReport.GetDataSourceNames();
            Debug.Assert(expected.Count > 0);

            foreach (DataTable table in dataset.Tables)
            {
                Microsoft.Reporting.WinForms.ReportDataSource rptDataSource = new Microsoft.Reporting.WinForms.ReportDataSource
                {
                    Name = table.TableName,
                    Value = table,
                };

                localReport.DataSources.Add(rptDataSource);
            }
        }

        private void AddDataSources(LocalReport localReport, ReportDefinition report)
        {
            string[] sources = localReport.GetDataSourceNames().ToArray();
            Debug.Assert(sources.Length > 0);

            Debug.Assert(localReport.DataSources.Count == report.DataSources.Count);

            foreach (var item in report.DataSources)
            {
                Log($"{item.Name} = {item.ConnectString}");
            }
        }

        private void Log(string message)
        {
            Trace.WriteLine(message);
            SendMessage?.Invoke(message);
        } 

 
    }
}
