using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace KafkaWindowsServiceWrapper
{
    /// <summary>
    /// Helps with installing and uninstalling services.
    /// </summary>
    /// <remarks>
    /// Code adapted from http://stackoverflow.com/questions/1195478/how-to-make-a-net-windows-service-start-right-after-the-installation/1195621#1195621
    /// </remarks>
    public static class InstallUtils
    {
        private static bool IsInstalled(string serviceName)
        {
            using (ServiceController controller =
                new ServiceController(serviceName))
            {
                try
                {
                    ServiceControllerStatus status = controller.Status;
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }


        private static bool IsRunning(string serviceName)
        {
            using (ServiceController controller =
                new ServiceController(serviceName))
            {
                if (!IsInstalled(serviceName))
                {
                    return false;
                }

                return (controller.Status == ServiceControllerStatus.Running);
            }
        }

        private static AssemblyInstaller GetInstaller(Assembly assembly)
        {
            AssemblyInstaller installer = new AssemblyInstaller(assembly, null);
            installer.UseNewContext = true;
            return installer;
        }

        /// <summary>
        /// Installs the services defined in the given assembly.
        /// </summary>
        /// <param name="assembly">An assembly containing Windows services.</param>
        public static void InstallServices(Assembly assembly)
        {
            try
            {
                using (AssemblyInstaller installer = GetInstaller(assembly))
                {
                    IDictionary state = new Hashtable();
                    try
                    {
                        installer.Install(state);
                        installer.Commit(state);
                    }
                    catch
                    {
                        try
                        {
                            installer.Rollback(state);
                        }
                        catch { }
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Uninstalls the services defined in the given assembly.
        /// </summary>
        /// <param name="assembly">An assembly containing Windows services.</param>
        public static void UninstallServices(Assembly assembly)
        {
            try
            {
                using (AssemblyInstaller installer = GetInstaller(assembly))
                {
                    IDictionary state = new Hashtable();
                    try
                    {
                        installer.Uninstall(state);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private static void StartService(string serviceName)
        {
            if (!IsInstalled(serviceName))
            {
                return;
            }

            using (ServiceController controller =
                new ServiceController(serviceName))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Running)
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running,
                            TimeSpan.FromSeconds(10));
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Stops a running service.
        /// </summary>
        /// <param name="serviceName">The name of the service to stop.</param>
        public static void StopService(string serviceName)
        {
            if (!IsInstalled(serviceName))
            {
                return;
            }

            using (ServiceController controller =
                new ServiceController(serviceName))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped,
                             TimeSpan.FromSeconds(10));
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
