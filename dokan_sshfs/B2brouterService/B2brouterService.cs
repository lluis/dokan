using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Dokan;

namespace DokanSSHFS
{
    public partial class B2brouterService : ServiceBase
    {
        private DokanSSHFS.SSHFS sshfs;
        private DokanOptions opt;
        private Settings settings = new Settings();
        private String user;
        private String drive;
        private String host;
        private Thread dokan;

        public B2brouterService()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("B2bService"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "B2bService", "B2bLog");
            }
            eventLog1.Source = "B2bService";
            eventLog1.Log = "B2bLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Starting B2brouter Service");
            sshfs = new SSHFS();
            opt = new DokanOptions();
            opt.DebugMode = Program.DokanDebug;
            opt.UseAltStream = true;
            opt.ThreadCount = 0;
            opt.UseKeepAlive = true;
            opt.VolumeLabel = "B2BRouter";
            settingsLoad();

            string message = "";

            if (user == "")
                message += "User name is empty\n";

            if (drive.Length != 1)
            {
                message += "Drive letter is invalid\n";
            }
            else
            {
                char letter = drive[0];
                letter = Char.ToLower(letter);
                if (!('e' <= letter && letter <= 'z'))
                    message += "Drive letter is invalid\n";

                opt.MountPoint = string.Format("{0}:\\", letter);
            }

            opt.ThreadCount = Program.DokanThread;

            if (message.Length != 0)
            {
                eventLog1.WriteEntry("ERROR: "+message);
                return;
            }

            Program.UseOffline = false;

            sshfs.Initialize(
                user,
                "compartit.b2brouter.net",
                22,
                null,
                B2BRouter.ProgramFilesx86() + "\\b2brouter\\b2brouter.key",
                "",
                "/incoming/",
                Program.SSHDebug);

            if (sshfs.SSHConnect())
            {
                MountWorker worker = null;
                worker = new MountWorker(new CacheOperations(sshfs), opt, eventLog1);
                dokan = new Thread(worker.Start);
                dokan.Start();
            }
            else
            {
                eventLog1.WriteEntry("ERROR: Can't connect");
                Stop();
                System.Threading.Thread.Sleep(1000);
                return;
            }
            eventLog1.WriteEntry("Connected");
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Stoping B2brouter Service");
            if (opt != null && sshfs != null)
            {
                Debug.WriteLine(string.Format("SSHFS Trying unmount : {0}", opt.MountPoint));

                if (DokanNet.DokanRemoveMountPoint(opt.MountPoint) < 0)
                {
                    Debug.WriteLine("DokanReveMountPoint failed\n");
                    // If DokanUmount failed, call sshfs.Unmount to disconnect.
                    // sshfs.Unmount(null);
                }
                else
                {
                    Debug.WriteLine("DokanReveMountPoint success\n");
                }
                // This should be called from Dokan, but not called.
                // Call here explicitly.
                sshfs.Unmount(null);
            }
        }

        private void settingsLoad()
        {
            settings.Load();
            Setting s = settings[0];
            user = s.User;
            drive = s.Drive;
            host = s.Host;
            eventLog1.WriteEntry("Settings loaded: "+user+"@"+host+" -> "+drive);
        }
    }

    class MountWorker
    {
        private DokanOperations sshfs_;
        private DokanOptions opt_;
        private EventLog logger_;

        public MountWorker(DokanOperations sshfs, DokanOptions opt, EventLog logger)
        {
            sshfs_ = sshfs;
            opt_ = opt;
            logger_ = logger;
        }

        public void Start()
        {
            //System.IO.Directory.SetCurrentDirectory(Application.StartupPath);
            int ret = DokanNet.DokanMain(opt_, sshfs_);
            if (ret < 0)
            {
                string msg = "Dokan Error";
                switch (ret)
                {
                    case DokanNet.DOKAN_ERROR:
                        msg = "Dokan Error";
                        break;
                    case DokanNet.DOKAN_DRIVE_LETTER_ERROR:
                        msg = "Dokan drive letter error";
                        break;
                    case DokanNet.DOKAN_DRIVER_INSTALL_ERROR:
                        msg = "Dokan driver install error";
                        break;
                    case DokanNet.DOKAN_MOUNT_ERROR:
                        msg = "Dokan drive letter assign error";
                        break;
                    case DokanNet.DOKAN_START_ERROR:
                        msg = "Dokan driver error ,please reboot";
                        break;
                }
                logger_.WriteEntry("ERROR: Can't connect ("+msg+")");
            }
            Debug.WriteLine("DokanNet.Main end");
        }
    }
}
