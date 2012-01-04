using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Dokan;
using System.Threading;

namespace DokanSSHFS
{
    partial class SSHFSService : ServiceBase
    {
        private SSHFS sshfs;
        private DokanOptions opt;
        private Settings settings = new Settings();
        private Setting curr_settings = null;
        private Thread dokan;

        public SSHFSService()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("SSHFSService"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "SSHFSService", "Application");
            }
            eventLog1.Source = "SSHFSService";
            eventLog1.Log = "Application";
            settingsLoad();
        }

        protected override void OnStart(string[] args)
        {
            sshfs = new SSHFS(eventLog1);
            opt = new DokanOptions();

            opt.DebugMode = DokanSSHFS.DokanDebug;
            opt.UseAltStream = true;
            opt.MountPoint = "n:\\";
            opt.ThreadCount = 0;
            opt.UseKeepAlive = true;

            opt.ThreadCount = DokanSSHFS.DokanThread;

            DokanSSHFS.UseOffline = !curr_settings.WithoutOfflineAttribute;

            sshfs.Initialize(
                curr_settings.User,
                curr_settings.Host,
                curr_settings.Port,
                curr_settings.UsePassword ? "" : null, //TODO - password
                curr_settings.UsePassword ? null : curr_settings.PrivateKey,
                null, //TODO - passphrase",
                curr_settings.ServerRoot,
                DokanSSHFS.SSHDebug);

            if (sshfs.SSHConnect())
            {
                MountWorker worker = null;
                if (curr_settings.DisableCache)
                {
                    worker = new MountWorker(sshfs, opt);
                }
                else
                {
                    worker = new MountWorker(new CacheOperations(sshfs), opt);
                }

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
        }

        protected override void OnStop()
        {
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
            curr_settings = settings[0]; //TODO
        }
    }
}
