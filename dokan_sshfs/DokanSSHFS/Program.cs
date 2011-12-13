using System;
using System.Threading;
using System.Windows.Forms;
using Dokan;

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
            //ConsoleWin.Open();

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
//			Application.Run(new SettingForm());
//			Application.Run(new Systray());

//            /*
            ParseArgs parser = new ParseArgs();
            parser.parse(args);

            if (!parser.CheckParam())
            {
                parser.help();
                return;
            }
			
			Application.Run(new Systray(parser));

//            DokanOptions opt = new DokanOptions();
//
//            opt.DebugMode = parser.debug;
//            opt.MountPoint = parser.drive;
//            opt.ThreadCount = parser.threads;
//
//			
//			// string user, string host, int port, string password, string identity,
//			// string passphrase, string root, bool debug
//            SSHFS sshfs = new SSHFS();
//			sshfs.Initialize(parser.user,
//                parser.host, parser.port, null, parser.identity, null, parser.root, parser.debug);
//
//            if (sshfs.SSHConnect())
//            {
//                DokanNet.DokanMain(opt, sshfs);
//            }
//            else
//            {
//                Console.Error.WriteLine("failed to connect");
//            }
//            Console.Error.WriteLine("sshfs exit");
			
//             */
        }
    }
}
