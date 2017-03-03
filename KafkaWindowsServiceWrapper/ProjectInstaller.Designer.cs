namespace KafkaWindowsServiceWrapper
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.zooKeeperServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.kafkaServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // zooKeeperServiceInstaller
            // 
            this.zooKeeperServiceInstaller.Description = "ZooKeeper is a centralized service for maintaining configuration information.";
            this.zooKeeperServiceInstaller.DisplayName = "Apache ZooKeeper";
            this.zooKeeperServiceInstaller.ServiceName = "ZooKeeper";
            // 
            // kafkaServiceInstaller
            // 
            this.kafkaServiceInstaller.Description = "Kafka™ is used for building real-time data pipelines and streaming apps.";
            this.kafkaServiceInstaller.DisplayName = "Apache Kafka";
            this.kafkaServiceInstaller.ServiceName = "Kafka";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.zooKeeperServiceInstaller,
            this.kafkaServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller zooKeeperServiceInstaller;
        private System.ServiceProcess.ServiceInstaller kafkaServiceInstaller;
    }
}