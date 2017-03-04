using System;
using System.ServiceProcess;

namespace KafkaWindowsServiceWrapper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                HandleArgument(args[0]);
                return;
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ZooKeeper(),
                new Kafka()
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static void HandleArgument(string argument)
        {
            switch (argument)
            {
                case "-install":
                    Install();
                    break;
                case "-uninstall":
                    Uninstall();
                    break;
                default:
                    Environment.ExitCode = 1;
                    break;
            }
        }

        private static void Uninstall()
        {
            InstallUtils.StopService("Kafka");
            InstallUtils.StopService("ZooKeeper");
            InstallUtils.UninstallServices(typeof(Program).Assembly);
        }

        private static void Install()
        {
            InstallUtils.InstallServices(typeof(Program).Assembly);
        }
    }
}
