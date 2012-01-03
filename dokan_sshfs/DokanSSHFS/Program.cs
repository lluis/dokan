using System;
using System.Threading;
using System.Windows.Forms;
//using Dokan;

namespace DokanSSHFS
{
    class DokanSSHFS
    {
        public static bool DokanDebug = false;
        public static bool SSHDebug = false;
        public static ushort DokanThread = 0;
        public static bool UseOffline = true;

        [STAThread]
        static void Main()
        {
//            ConsoleWin.Open();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			B2brouterServiceManager form = new B2brouterServiceManager();
			form.SettingLoad();
			Application.Run(form);
        }
    }
}
