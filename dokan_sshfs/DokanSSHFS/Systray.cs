using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Dokan;
using System.Diagnostics;


namespace DokanSSHFS
{
	public class Systray : Form
	{
		private SSHFS sshfs;
        private DokanOptions opt;
        private Thread dokan;
        private bool isUnmounted_ = false;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenuStrip notifyMenu;
		private System.Windows.Forms.ToolStripMenuItem exit;
        private System.Windows.Forms.ToolStripMenuItem unmount;
        private System.Windows.Forms.ToolStripMenuItem mount;
		public ParseArgs parser;
		
		public Systray(ParseArgs parser)
		{
			this.parser = parser;
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Systray));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.unmount = new System.Windows.Forms.ToolStripMenuItem();
            this.mount = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyMenu.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.notifyMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "SSHFS";
            this.notifyIcon1.Visible = true;
            // 
            // notifyMenu
            // 
            this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exit,
            this.unmount,
            this.mount});
            this.notifyMenu.Name = "Exit";
            this.notifyMenu.ShowImageMargin = false;
            this.notifyMenu.Size = new System.Drawing.Size(91, 70);
            // 
            // exit
            // 
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(90, 22);
            this.exit.Text = "Exit";
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // unmount
            // 
            this.unmount.Name = "unmount";
            this.unmount.Size = new System.Drawing.Size(90, 22);
            this.unmount.Text = "Unmount";
            this.unmount.Visible = false;
            this.unmount.Click += new System.EventHandler(this.unmount_Click);
            // 
            // mount
            // 
            this.mount.Name = "mount";
            this.mount.Size = new System.Drawing.Size(90, 22);
            this.mount.Text = "Mount";
            this.mount.Click += new System.EventHandler(this.mount_Click);
			//
			this.notifyMenu.ResumeLayout(false);
			this.PerformLayout();
		}
		
		protected override void OnLoad(EventArgs e)
        {
            Visible       = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
			connect();

            base.OnLoad(e);
        }
		
		private void connect() {
			int p = 22;

            sshfs = new SSHFS();
            opt = new DokanOptions();

            opt.DebugMode = parser.debug;
            opt.UseAltStream = true;
//            opt.MountPoint = "n:\\";
            opt.ThreadCount = 0;
            opt.UseKeepAlive = true;

            string message = "";

            if (parser.host == "")
                message += "Host name is empty\n";

            if (parser.user == "")
                message += "User name is empty\n";

            if (parser.drive.Length != 1)
            {
                message += "Drive letter is invalid\n";
            }
            else
            {
                char letter = parser.drive[0];
                letter = Char.ToLower(letter);
                if (!('e' <= letter && letter <= 'z'))
                    message += "Drive letter is invalid\n";

                opt.MountPoint = string.Format("{0}:\\", letter);
                unmount.Text = "Unmount (" + opt.MountPoint + ")";
            }

            opt.MountPoint = string.Format("{0}:\\", 'r');
            unmount.Text = "Unmount (" + opt.MountPoint + ")";
			opt.UseStdErr = parser.debug;
			
            opt.ThreadCount = DokanSSHFS.DokanThread;

            if (message.Length != 0)
            {
                MessageBox.Show(message, "Error");
                return;
            }

//            DokanSSHFS.UseOffline = !withoutOfflineAttribute.Checked;
            DokanSSHFS.UseOffline = false;			

			//TODO: Add password auth and key passphrase support
            sshfs.Initialize(
                parser.user,
                parser.host,
                p,
                null,
                parser.identity,
                "",
                parser.root,
                DokanSSHFS.SSHDebug);

            if (sshfs.SSHConnect())
            {
                unmount.Visible = true;
                mount.Visible = false;
                isUnmounted_ = false;

                MountWorker worker = null;
//                if (disableCache.Checked)
//                {
                    worker = new MountWorker(sshfs, opt);
//                }
//                else
//                {
//                    worker = new MountWorker(new CacheOperations(sshfs), opt);
//                }

                dokan = new Thread(worker.Start);
                dokan.Start();
            }
            else
            {
                MessageBox.Show("failed to connect", "Error");
                return;
            }

//            MessageBox.Show("sshfs start", "info");
		}

		private void mount_Click(object sender, EventArgs e)
        {
            unmount.Visible = false;
			connect();
        }
		
		private void unmount_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("unmount_Click");          
            this.Unmount();
            isUnmounted_ = true;
        }
		
        private void exit_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;

            if (!isUnmounted_)
            {
                Debug.WriteLine("unmount is visible");
                unmount.Visible = false;
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
            unmount.Visible = false;
            mount.Visible = true;
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
                            msg = "Dokan drive letter error: " + opt_.MountPoint;
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

	}
}

