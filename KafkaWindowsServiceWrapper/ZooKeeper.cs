using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace KafkaWindowsServiceWrapper
{
    /// <summary>
    /// The ZooKeeper service.
    /// </summary>
    public partial class ZooKeeper : BatchFileServiceBase
    {
        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        public ZooKeeper()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the command line that starts the service.
        /// </summary>
        /// <returns></returns>
        protected override string GetStartBatchFile()
        {
            return Utils.GetCommandLine(@"bin\windows\zookeeper-server-start.bat", @"config\zookeeper.properties");
        }

        /// <summary>
        /// Gets the command line that stops the service.
        /// </summary>
        /// <returns></returns>
        protected override string GetStopBatchFile()
        {
            return Utils.GetCommandLine(@"bin\windows\zookeeper-server-stop.bat", @"config\zookeeper.properties");
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            WaitForPortOpen();
        }

        /// <summary>
        /// Waits until the ZooKeeper port is listening to connections.
        /// This way, we don't start Kafka too soon.
        /// </summary>
        private void WaitForPortOpen()
        {
            bool connected = false;
            int attempts = 0;
            while (!connected && attempts < MaxConnectAttemptCount)
            {
                attempts++;
                EventLog.WriteEntry("Testing if ZooKeeper is listening, attempt #" + attempts);

                connected = IsPortOpen();
                if (!connected)
                {
                    if (attempts >= MaxConnectAttemptCount)
                    {
                        EventLog.WriteEntry("ZooKeeper port not listening, giving up", EventLogEntryType.Error);
                        throw new InvalidOperationException("ZooKeeper port not listening");
                    }
                    else
                    {
                        Thread.Sleep(ConnectTimeout);
                    }
                }
            }
        }

        private bool IsPortOpen()
        {
            bool isPortOpen = false;
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    var result = tcpClient.BeginConnect(Host, Port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(ConnectTimeout);
                    if (success)
                    {
                        tcpClient.EndConnect(result);
                    }

                    isPortOpen = true;
                }
                catch (SocketException ex)
                {
                    EventLog.WriteEntry("ZooKeeper port not listening: " + ex.Message, EventLogEntryType.Information);
                }
            }

            return isPortOpen;
        }

        private string Host
        {
            get
            {
                return ConfigurationManager.AppSettings["ZooKeeperHost"];
            }
        }

        private int Port
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["ZooKeeperPort"]);
            }
        }

        private TimeSpan ConnectTimeout
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["ZooKeeperConnectTimeout"]));
            }
        }

        private int MaxConnectAttemptCount
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["ZooKeeperMaxConnectAttemptCount"]);
            }
        }
    }
}
