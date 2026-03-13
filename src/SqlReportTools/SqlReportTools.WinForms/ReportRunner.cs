using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SqlReportTools.WinForms
{
    /// <summary>Coordinates between</summary>
    public class ReportRunner
    {
        private Settings Settings { get; }
        private Action<string> Logger { get; }

        public ReportManager RdlManager { get; } 
        public DataSourceManager RdsManager { get; } 


        public ReportRunner(Settings settings, Action<string> logger)
        {
            Settings = settings;
            Logger = logger;
 
            RdlManager = new ReportManager(Logger);
            RdsManager = new DataSourceManager(Logger);
        }

        public void ScanFiles()
        {
            RdsManager.ScanRdsFiles(Settings.SqlReportsDirectory);
            RdlManager.AddReports(RdsManager, Settings.SqlReportsDirectory);
        }

        public ReportDefinition GetReportDefinition(FileInfo reportFile)
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

        public bool Run(ReportViewer viewer, ReportDefinition report)
        {
            if (report == null || report.File == null || !report.File.Exists)
            {
                return false;
            }

            // Set the processing mode for the ReportViewer to Local  
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = report.File.FullName;

            var localReport = viewer.LocalReport;
            AddReportParameter(report, localReport);

            // -------------------------
            // Data Sets
            // -------------------------
            try
            {
                var dataSet = report.FillDataSets(Log, Settings.DevSqlServer);
                AddDataSetToLocalReport(localReport, dataSet);

                AddDataSources(localReport, report);
                return true;
            }
            catch(Exception ex)
            {
                Log($"Exception: {ex.GetType().Name} {ex.Message}");
                return false;
            }
            finally
            {
                // Refresh the report  
                viewer.RefreshReport();
            }
        }

        /// <summary>Add Parameters to the Report</summary>
        private void AddReportParameter(ReportDefinition report, LocalReport localReport)
        {
            foreach (ReportParameterInfo parm in localReport.GetParameters())
            {
                Log($"Parameter: {parm.Name} = {parm.DataType}");
                var rptParm = report.Parameters.FirstOrDefault(x => x.Name == parm.Name);
                if(rptParm != null)
                {
                    parm.Values = rptParm.Value?.Split(',').ToList() ?? rptParm.DefaultValues;
                }
            }
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

        [DebuggerStepThrough]
        private void Log(string message)
        {
            Trace.WriteLine(message);
            Logger?.Invoke(message);
        } 

 
    }
}
