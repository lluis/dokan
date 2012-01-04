using System;
using System.Threading;
using System.Windows.Forms;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace DokanSSHFS
{
    class DokanSSHFS
    {
        public static bool DokanDebug = false;
        public static bool SSHDebug = false;
        public static ushort DokanThread = 0;
        public static bool UseOffline = true;

        //[STAThread]
        static void Main(String[] args)
        {
            if (System.Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "--installservice":
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    case "--uninstallservice":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                    default:
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new SettingForm());
                        break;
                }
            }
            else
            {
                ServiceBase.Run(new SSHFSService());
            }
        }
    }
}
