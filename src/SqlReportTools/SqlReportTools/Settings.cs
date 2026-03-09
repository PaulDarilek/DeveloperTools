using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SqlReportTools
{
    public class Settings
    {
        /// <summary>Location of SSRS *.rdl and *.rds files</summary>
        public DirectoryInfo SqlReportsDirectory { get; set; }

        /// <summary>Directory for Output of Cross References reports and documentation</summary>
        public DirectoryInfo OutputDirectory { get; set; }

        /// <summary></summary>
        /// <param name="config"></param>
        public Settings(IConfiguration config)
        {
            config = config.GetSection("AppSettings");
            var path = config[nameof(SqlReportsDirectory)];
            if (!string.IsNullOrWhiteSpace(path))
            {
                SqlReportsDirectory = new DirectoryInfo(path);
            }
            else
            {
                SqlReportsDirectory = GetReportsDirectory();
            }

            path = config[nameof(OutputDirectory)];
            if (!string.IsNullOrWhiteSpace(path))
            {
                OutputDirectory = new DirectoryInfo(path);
            }
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
