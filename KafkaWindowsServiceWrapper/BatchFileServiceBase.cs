using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaWindowsServiceWrapper
{
    /// <summary>
    /// A base class for services that are wrappers for batch files.
    /// Such a service is started and stopped by a batch file.
    /// </summary>
    public class BatchFileServiceBase : ServiceBase
    {
        /// <summary>
        /// Holds the underlying service process.
        /// </summary>
        private Process process;

        /// <summary>
        /// Indicates if we're currently shutting down the windows service.
        /// Useful to distinguish between an expected and unexpected termination of the underlying service.
        /// </summary>
        private bool exiting;

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // clean up previously hanging process
            OnStop();

            // start the process
            process = RunBatchFile(GetStartBatchFile());
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                // indicate we're shutting down the service on purpose
                exiting = true;

                var stopProcess = RunBatchFile(GetStopBatchFile());
                stopProcess.WaitForExit();

                if (process != null)
                {
                    process.WaitForExit();
                    process = null;
                }
            }
            finally
            {
                exiting = false;
            }
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
        /// Code based on https://github.com/lukemerrett/Kafka-Windows-Service
        /// </remarks>
        private Process RunBatchFile(string command)
        {
            var process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = Utils.KafkaInstallationDirectory;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + command;
            process.EnableRaisingEvents = true;
            process.Exited += (sender, e) =>
            {
                if (exiting)
                {
                    EventLog.WriteEntry("Process exited: " + command);
                }
                else
                {
                    EventLog.WriteEntry("Process exited unexpectedly: " + command, EventLogEntryType.Error);

                    // stop the windows service to align with the underlying service
                    Stop();
                }
            };

            process.Start();
            Thread.Sleep(CoolingTimeout);
            return process;
        }

        private TimeSpan CoolingTimeout
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["CoolingTimeout"]));
            }
        }
    }
}
