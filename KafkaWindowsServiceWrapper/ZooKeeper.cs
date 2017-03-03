using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace KafkaWindowsServiceWrapper
{
    public partial class ZooKeeper : BatchFileServiceBase
    {
        public ZooKeeper()
        {
            InitializeComponent();
        }

        protected override string GetStartBatchFile()
        {
            return Utils.GetCommandLine(@"bin\windows\zookeeper-server-start.bat", @"config\zookeeper.properties");
        }

        protected override string GetStopBatchFile()
        {
            return Utils.GetCommandLine(@"bin\windows\zookeeper-server-stop.bat", @"config\zookeeper.properties");
        }
    }
}
