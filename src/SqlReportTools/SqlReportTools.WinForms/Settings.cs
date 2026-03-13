using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SqlReportTools.WinForms
{
    public class Settings
    {
        /// <summary>Location of SSRS *.rdl and *.rds files</summary>
        public DirectoryInfo SqlReportsDirectory { get; set; }

        /// <summary>Directory for Output of Cross References reports and documentation</summary>
        public DirectoryInfo OutputDirectory { get; set; }

        /// <summary>Optional Sql Server to over-ride the connection string</summary>
        public string SqlServer { get; set; }

        /// <summary></summary>
        /// <param name="config"></param>
        public Settings(IConfiguration config)
        {
            config = config.GetSection("AppSettings") ?? config;
            
            var value = config[nameof(SqlReportsDirectory)];
            value = string.IsNullOrEmpty(value) ? Environment.CurrentDirectory : value;
            SqlReportsDirectory = new DirectoryInfo(value);

            value = config[nameof(OutputDirectory)];
            if (!string.IsNullOrWhiteSpace(value))
            {
                OutputDirectory = new DirectoryInfo(value);
            }

            value = config[nameof(SqlServer)];
            if (!string.IsNullOrWhiteSpace(value))
            {
                SqlServer = value;
            }

        }

    }
}
