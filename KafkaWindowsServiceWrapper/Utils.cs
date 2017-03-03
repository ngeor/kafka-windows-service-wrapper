using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaWindowsServiceWrapper
{
    public static class Utils
    {
        public static string KafkaInstallationDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["KafkaInstallationDirectory"];
            }
        }

        public static string GetCommandLine(string batchFile, string configFile)
        {
            return Path.Combine(KafkaInstallationDirectory, batchFile) +
                " " +
                Path.Combine(KafkaInstallationDirectory, configFile);
        }
    }
}
