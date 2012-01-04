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
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                }
            }
            else
            {
                ServiceBase.Run(new B2brouterService());
            }

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
			//{ 
			//	new B2brouterService() 
			//};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
