using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SqlReportTools.WinForms
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var settings = new Settings(GetConfiguration());
            if (settings.SqlReportsDirectory == null || !settings.SqlReportsDirectory.Exists)
            {
                settings.SqlReportsDirectory = GetReportsDirectory();
            }

            var reportRunner = new ReportRunner(settings);

            Application.Run(new ViewReport(reportRunner));
        }

        private static IConfiguration GetConfiguration()
        {
            IConfiguration configuration =
                new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();
            return configuration;
        }
        private static DirectoryInfo GetReportsDirectory()
        {
            var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
            for (int i = 0; i < 7 && dirInfo != null; i++)
            {
                var slnFile =
                    dirInfo.EnumerateFiles("*.sln").FirstOrDefault() ??
                    dirInfo.EnumerateFiles("*.slnx").FirstOrDefault();

                if (slnFile != null)
                {
                    return dirInfo;
                }

                dirInfo = dirInfo.Parent;
            }
            return new DirectoryInfo(Environment.CurrentDirectory);
        }

    }
}
