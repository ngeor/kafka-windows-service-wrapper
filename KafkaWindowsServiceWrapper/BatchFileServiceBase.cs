using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace KafkaWindowsServiceWrapper
{
    public class BatchFileServiceBase : ServiceBase
    {
        private Process process;

        protected override void OnStart(string[] args)
        {
            process = RunBatchFile(GetStartBatchFile());
        }

        protected override void OnStop()
        {
            var stopProcess = RunBatchFile(GetStopBatchFile());
            stopProcess.WaitForExit();
        }

        /// <summary>
        /// Gets the batch file that starts the service.
        /// </summary>
        /// <remarks>
        /// This should have been an abstract method, but the Visual Studio designer won't load if the base class is abstract.
        /// </remarks>
        /// <returns></returns>
        protected virtual string GetStartBatchFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the batch file that stops the service.
        /// </summary>
        /// <remarks>
        /// This should have been an abstract method, but the Visual Studio designer won't load if the base class is abstract.
        /// </remarks>
        /// <returns></returns>
        protected virtual string GetStopBatchFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Runs a batch file and returns the process.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>
        /// Code copied from https://github.com/lukemerrett/Kafka-Windows-Service
        /// </remarks>
        private Process RunBatchFile(string command)
        {
            ProcessStartInfo processInfo;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = Utils.KafkaInstallationDirectory
            };

            return Process.Start(processInfo);
        }
    }
}
