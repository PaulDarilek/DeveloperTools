using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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

            Application.Run(new ViewReport(settings));
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
    }
}
