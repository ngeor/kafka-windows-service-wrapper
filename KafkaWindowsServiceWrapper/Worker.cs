using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaWindowsServiceWrapper
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private Process? _process;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _logger.LogInformation("Instantiated worker");
            _configuration = configuration;
        }

        public void Dispose()
        {
            _process?.Dispose();
            _process = null;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_process != null)
            {
                if (_process.HasExited)
                {
                    _process.Dispose();
                    _process = null;
                }
                else
                {
                    _logger.LogWarning("Unexpected running process found");
                }
            }
            _process = IsZookeeper() ? CreateProcess(@"bin\windows\zookeeper-server-start.bat", @"config\zookeeper.properties") : CreateProcess(@"bin\windows\kafka-server-start.bat", @"config\server.properties");
            Thread.Sleep(2000);
            return _process.HasExited ? Task.FromException(new Exception("Could not start app")) : Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_process != null)
            {
                if (_process.HasExited)
                {
                    _process.Dispose();
                    _process = null;
                }
                else
                {
                    _logger.LogInformation("Stopping service");
                    var stopProcess = IsZookeeper() ? CreateProcess(@"bin\windows\zookeeper-server-stop.bat", @"config\zookeeper.properties") : CreateProcess(@"bin\windows\kafka-server-stop.bat", @"config\server.properties");
                    stopProcess.WaitForExit();
                    _process.WaitForExit();
                    _process.Dispose();
                    _process = null;
                }
            }
            return Task.CompletedTask;
        }

        private Process CreateProcess(string relativeBatchFile, string relativeConfigFile)
        {
            var directory = _configuration.GetValue("KafkaDirectory", "");
            var javaHome = _configuration.GetValue("JavaHome", "");
            var batchFile = Path.Combine(directory, relativeBatchFile);
            var configFile = Path.Combine(directory, relativeConfigFile);
            var args = string.Format("/c {0} {1}", batchFile, configFile);
            _logger.LogInformation("Starting process, directory = {}, args = {}", directory, args);
            var process = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = directory,
                    FileName = "cmd.exe",
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                },
                EnableRaisingEvents = true
            };
            process.Exited += (sender, e) =>
            {
                _logger.LogInformation("Process {} exited {}", relativeBatchFile, process.ExitCode);
            };
            process.OutputDataReceived += (sender, e) =>
            {
                _logger.LogInformation("OUTPUT: {}", e.Data);
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                _logger.LogWarning("ERROR: {}", e.Data);
            };
            if (!string.IsNullOrWhiteSpace(javaHome))
            {
                process.StartInfo.Environment.Add("JAVA_HOME", javaHome);
            }
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            return process;
        }

        private bool IsZookeeper()
        {
            var name = _configuration.GetValue("Name", "");
            return name.Contains("Zookeeper", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
