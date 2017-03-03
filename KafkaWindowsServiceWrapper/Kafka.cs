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
    partial class Kafka : BatchFileServiceBase
    {
        public Kafka()
        {
            InitializeComponent();
        }

        protected override string GetStartBatchFile()
        {
            return Utils.GetCommandLine(@"bin\windows\kafka-server-start.bat", @"config\server.properties");
        }

        protected override string GetStopBatchFile()
        {
            return Utils.GetCommandLine(@"bin\windows\kafka-server-stop.bat", @"config\server.properties");
        }        
    }
}
