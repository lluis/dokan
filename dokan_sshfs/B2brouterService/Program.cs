using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

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
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new B2brouterService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
