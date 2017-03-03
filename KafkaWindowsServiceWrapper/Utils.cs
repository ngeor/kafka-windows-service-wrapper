using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaWindowsServiceWrapper
{
    /// <summary>
    /// Utility functions regarding the Kafka installation.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Gets the installation directory of Kafka.
        /// </summary>
        /// <remarks>
        /// The installation directory is defined in the configuration file of the application.
        /// </remarks>
        public static string KafkaInstallationDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["KafkaInstallationDirectory"];
            }
        }

        /// <summary>
        /// Creates a command line consisting of a batch file and a configuration file.
        /// </summary>
        /// <param name="batchFile"></param>
        /// <param name="configFile"></param>
        /// <returns></returns>
        /// <remarks>
        /// Typically the Kafka batch files accept their configuration file as a parameter.
        /// </remarks>
        public static string GetCommandLine(string batchFile, string configFile)
        {
            return Path.Combine(KafkaInstallationDirectory, batchFile) +
                " " +
                Path.Combine(KafkaInstallationDirectory, configFile);
        }
    }
}
