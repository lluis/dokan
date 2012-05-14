namespace DokanSSHFS
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
            this.b2brouterProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.b2brouterInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // b2brouterProcessInstaller
            // 
            this.b2brouterProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.b2brouterProcessInstaller.Password = null;
            this.b2brouterProcessInstaller.Username = null;
            this.b2brouterProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.b2brouterProcessInstaller_AfterInstall);
            // 
            // b2brouterInstaller
            // 
            this.b2brouterInstaller.ServiceName = "B2brouterService";
            this.b2brouterInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.b2brouterInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.b2brouterInstaller_AfterInstall);
            this.b2brouterInstaller.ServicesDependedOn = new string[] { "Tcpip" };
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.b2brouterProcessInstaller,
            this.b2brouterInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller b2brouterProcessInstaller;
        private System.ServiceProcess.ServiceInstaller b2brouterInstaller;
    }
}