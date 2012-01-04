using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration.Install;
using System.Reflection;

namespace DokanSSHFS
{
    static class Program
    {
        public static bool DokanDebug = false;
        public static bool SSHDebug = false;
        public static ushort DokanThread = 0;
        public static bool UseOffline = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "--install":
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    case "--uninstall":
                        stop_Service();
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                }
            }
            else
            {
                ServiceBase.Run(new B2brouterService());
            }
        }

        static void stop_Service()
        {
            ServiceController b2bservice = new ServiceController();
            b2bservice.ServiceName = "B2brouterService";
            String srvstatus = b2bservice.Status.ToString();
            if (srvstatus == "Running")
            {
                b2bservice.Stop();
                int i = 0;
                while (srvstatus != "Stopped" && i < 10)
                {
                    b2bservice.Refresh();
                    srvstatus = b2bservice.Status.ToString();
                    System.Threading.Thread.Sleep(1000);
                    i += 1;
                }
            }
        }
    }
}
