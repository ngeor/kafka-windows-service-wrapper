using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaWindowsServiceWrapper
{
    public static class Utils
    {
        private static string KafkaInstallationDirectory
        {
            get
            {
                return @"C:\ecommerce\kafka_2.11-0.10.1.0";
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
