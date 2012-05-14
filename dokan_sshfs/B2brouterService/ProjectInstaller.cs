using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace DokanSSHFS
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void b2brouterProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void b2brouterInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
