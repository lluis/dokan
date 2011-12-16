using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Dokan;

namespace DokanSSHFS
{
    public partial class B2brouter : Form
    {
        private SSHFS sshfs;
        private DokanOptions opt;
		public Settings settings = new Settings();
        private Thread dokan;
        private bool isUnmounted_ = true;

        public B2brouter()
        {
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            //notifyIcon1.Icon = SystemIcons.Application;
            notifyIcon1.Visible = true;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
			this.Hide();
        }

        public void connect_Click(object sender, EventArgs e)
        {
			this.Hide();
			settingSave();

			if (!isUnmounted_)
			{
				this.Unmount();
            	isUnmounted_ = true;
			}
			
            sshfs = new SSHFS();
            opt = new DokanOptions();

            opt.DebugMode = DokanSSHFS.DokanDebug;
            opt.UseAltStream = true;
            opt.MountPoint = "n:\\";
            opt.ThreadCount = 0;
            opt.UseKeepAlive = true;
			opt.VolumeLabel = "B2BRouter";

            string message = "";

            if (user.Text == "")
                message += "User name is empty\n";

            if (drive.Text.Length != 1)
            {
                message += "Drive letter is invalid\n";
            }
            else
            {
                char letter = drive.Text[0];
                letter = Char.ToLower(letter);
                if (!('e' <= letter && letter <= 'z'))
                    message += "Drive letter is invalid\n";

                opt.MountPoint = string.Format("{0}:\\", letter);
            }

            opt.ThreadCount = DokanSSHFS.DokanThread;

            if (message.Length != 0)
            {
                this.Show();
                MessageBox.Show(message, "Error");
                return;
            }

            DokanSSHFS.UseOffline = false;

            sshfs.Initialize(
                user.Text,
                "compartit.b2brouter.net",
                22,
                null,
                Environment.ExpandEnvironmentVariables("%ProgramFiles%\\b2brouter\\b2brouter.key"),
                "",
                "/incoming/",
                DokanSSHFS.SSHDebug);

            if (sshfs.SSHConnect())
            {
                isUnmounted_ = false;

                MountWorker worker = null;
                worker = new MountWorker(new CacheOperations(sshfs), opt);

				if (!settings[0].AutomaticStartup)
				{
					settings[0].AutomaticStartup = true;
					settings.Save();
				}

                dokan = new Thread(worker.Start);
                dokan.Start();
            }
            else
            {
                this.Show();
                MessageBox.Show("failed to connect", "Error");
                return;
            }

            //MessageBox.Show("sshfs start", "info");
            
        }


        private void Unmount()
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


        class MountWorker
        {
            private DokanOperations sshfs_;
            private DokanOptions opt_;
            
            public MountWorker(DokanOperations sshfs, DokanOptions opt)
            {
                sshfs_ = sshfs;
                opt_ = opt;
            }

            public void Start()
            {
                System.IO.Directory.SetCurrentDirectory(Application.StartupPath);
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
                    MessageBox.Show(msg, "Error");
                    Application.Exit();
                }
                Debug.WriteLine("DokanNet.Main end");
            }
        }

        
        private void exit_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;

            if (!isUnmounted_)
            {
                Debug.WriteLine("unmount is visible");
                Unmount();
                isUnmounted_ = true;
            }

            Debug.WriteLine("SSHFS Thread Waitting");

            if (dokan != null && dokan.IsAlive)
            {
                Debug.WriteLine("doka.Join");
                dokan.Join();
            }
            
            Debug.WriteLine("SSHFS Thread End");

            Application.Exit();
        }

        
		private void config_Click(object sender, EventArgs e)
		{
			Debug.WriteLine("config_Click");
			this.Show();
		}

		private void settingSave()
        {
            Setting s = settings[0];

            s.Name = "b2brouter";
            s.User = user.Text;
            s.Port = 22;
            s.Drive = drive.Text;

            settings.Save();
            SettingLoad();
        }
		
		public void SettingLoad()
        {
			settings.Load();
            Setting s = settings[0];

            user.Text = s.User;
            drive.Text = s.Drive;
        }

		public static bool IsProcessOpen()
		{
			Int32 i = 0;
		    foreach (Process clsProcess in Process.GetProcesses()) {
		        if (clsProcess.ProcessName.Contains("DokanSSHFS"))
		        {
					i += 1;
					// check if there are >1 processes with name DokanSSHFS
		            if (i > 1) {
						return true;
					}
		        }
		    }
		    return false;
		}
    }
}