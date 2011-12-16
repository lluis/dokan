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

			if (B2brouter.IsProcessOpen()) {
				MessageBox.Show("B2bRouter already running", "Error");
				return;
			}

			string[] args = System.Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg == "-sd")
                {
                    SSHDebug = true;
                }
                if (arg == "-dd")
                {
                    DokanDebug = true;
                }
                if (arg.Length >= 3 &&
                    arg[0] == '-' &&
                    arg[1] == 't')
                {
                    DokanThread = ushort.Parse(arg.Substring(2));
                }
                if (arg == "-no")
                {
                    UseOffline = false;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			B2brouter form = new B2brouter();
			form.SettingLoad();
			if (form.settings[0].AutomaticStartup) {
				form.Visible = false;
				form.connect_Click(null,null);
				Application.Run();
			} else {
				Application.Run(form);
			}
        }
    }
}
