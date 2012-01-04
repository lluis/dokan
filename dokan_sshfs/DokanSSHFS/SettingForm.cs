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
using System.ServiceProcess;

namespace DokanSSHFS
{
    public partial class SettingForm : Form
    {
        private DokanOptions opt;
        private Settings settings = new Settings();
        private int selectedIndex = 0;
        //private Thread dokan;
        ServiceController sshfsservice;
        String srvstatus = "";

        public SettingForm()
        {
            InitializeComponent();
            sshfsservice = new ServiceController();
            sshfsservice.ServiceName = "SSHFSService";
            Redraw();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            //notifyIcon1.Icon = SystemIcons.Application;
            SettingLoad();
        }

        private void open_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                privatekey.Text = openFileDialog1.FileName;
            }
            
        }

        private void usePassword_CheckedChanged(object sender, EventArgs e)
        {
            if (usePassword.Checked)
            {
                usePrivateKey.Checked = false;
                privatekey.Enabled = false;
                passphrase.Enabled = false;
                password.Enabled = true;
                open.Enabled = false;
            }
        }

        private void usePrivateKey_CheckedChanged(object sender, EventArgs e)
        {
            if (usePrivateKey.Checked)
            {
                usePassword.Checked = false;
                privatekey.Enabled = true;
                passphrase.Enabled = true;
                password.Enabled = false;
                open.Enabled = true;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void start_Click(object sender, EventArgs e)
        {
            this.progressBar1.Visible = true;
            int p = 22;
            
            opt = new DokanOptions();

            opt.DebugMode = DokanSSHFS.DokanDebug;
            opt.UseAltStream = true;
            opt.MountPoint = "n:\\";
            opt.ThreadCount = 0;
            opt.UseKeepAlive = true;

            string message = "";

            if (host.Text == "")
                message += "Host name is empty\n";

            if (user.Text == "")
                message += "User name is empty\n";


            if (port.Text == "")
                message += "Port is empty\n";
            else
            {
                try
                {
                    p = Int32.Parse(port.Text);
                }
                catch(Exception)
                {
                    message += "Port format error\n";
                }
            }

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
                this.progressBar1.Visible = false;
                MessageBox.Show(message, "Error");
                return;
            }

            DokanSSHFS.UseOffline = !withoutOfflineAttribute.Checked;

            SettingSave();

            srvstatus = sshfsservice.Status.ToString();

            message = null;
            if (srvstatus == "Stopped")
            {
                sshfsservice.Start();
                int i = 0;
                this.progressBar1.Value = i * 10;
                while (srvstatus != "Running" && i < 10)
                {
                    this.progressBar1.Value = i * 10;
                    sshfsservice.Refresh();
                    srvstatus = sshfsservice.Status.ToString();
                    System.Threading.Thread.Sleep(1000);
                    i += 1;
                }
                if (i >= 10)
                {
                    message = "Failed to connect.";
                }
            }
            else
            {
                // ???
            }
            Redraw(message);
            this.progressBar1.Visible = false;
        }

        private void stop_Click(object sender, EventArgs e)
        {
            this.progressBar1.Visible = true;
            srvstatus = sshfsservice.Status.ToString();
            if (srvstatus == "Running")
            {
                sshfsservice.Stop();
                int i = 0;
                while (srvstatus != "Stopped" && i < 10)
                {
                    this.progressBar1.Value = i * 10;
                    sshfsservice.Refresh();
                    srvstatus = sshfsservice.Status.ToString();
                    System.Threading.Thread.Sleep(1000);
                    i += 1;
                }
            }
            else
            {
                // ???
            }
            Redraw();
            this.progressBar1.Visible = false;
        }

        private void Redraw(String message = null)
        {
            String default_message = null;
            try
            {
                srvstatus = sshfsservice.Status.ToString();
            }
            catch { }
            if (srvstatus == "Running")
            {
                this.start.Enabled = false;
                this.stop.Enabled = true;
                this.label8.ForeColor = System.Drawing.Color.ForestGreen;
                default_message = "Service is currently running";
            }
            else if (srvstatus == "Stopped")
            {
                this.start.Enabled = true;
                this.stop.Enabled = false;
                this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaption;
                default_message = "Service is currently stopped";
            }
            else
            {
                this.start.Enabled = false;
                this.stop.Enabled = false;
                this.user.Enabled = false;
                this.drive.Enabled = false;
                this.label8.ForeColor = System.Drawing.Color.Firebrick;
                default_message = "Service is not correctly installed";
            }
            if (message != null)
            {
                this.label8.ForeColor = System.Drawing.Color.Firebrick;
                this.label8.Text = message;
            }
            else
            {
                this.label8.Text = default_message;
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
                            msg = "Dokan drive letter error" + opt_.MountPoint;
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
                } else {
					MessageBox.Show(opt_.MountPoint, "Error");
				}
                Debug.WriteLine("DokanNet.Main end");
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("SSHFS Thread Waitting");

            Application.Exit();
        }

        private void save_Click(object sender, EventArgs e)
        {
            Setting s = settings[selectedIndex];

            s.Name = settingNames.Text;
            if (settingNames.Text == "New Setting")
                s.Name = settings.GetNewName();

            s.Host = host.Text;
            s.User = user.Text;
            try
            {
                s.Port = Int32.Parse(port.Text);
            }
            catch (Exception)
            {
                s.Port = 22;
            }

            s.PrivateKey = privatekey.Text;
            s.UsePassword = usePassword.Checked;
            s.Drive = drive.Text;
            s.ServerRoot = root.Text;
            s.DisableCache = disableCache.Checked;
            s.WithoutOfflineAttribute = withoutOfflineAttribute.Checked;

            settings.Save();

            SettingLoad();
            SettingLoad(selectedIndex);
        }

        private void SettingSave()
        {
            Setting s = settings[selectedIndex];

            s.Name = settingNames.Text;
            if (settingNames.Text == "New Setting")
                s.Name = settings.GetNewName();

            s.Host = host.Text;
            s.User = user.Text;
            try
            {
                s.Port = Int32.Parse(port.Text);
            }
            catch (Exception)
            {
                s.Port = 22;
            }

            s.PrivateKey = privatekey.Text;
            s.UsePassword = usePassword.Checked;
            s.Drive = drive.Text;
            s.ServerRoot = root.Text;
            s.DisableCache = disableCache.Checked;
            s.WithoutOfflineAttribute = withoutOfflineAttribute.Checked;

            settings.Save();

            SettingLoad();
            SettingLoad(selectedIndex);
        }

        private void settingNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIndex = settingNames.SelectedIndex;
            SettingLoad(settingNames.SelectedIndex);
        }

        private void SettingLoad(int index)
        {
            Setting s = settings[index];

            host.Text = s.Host;
            user.Text = s.User;
            port.Text = s.Port.ToString();
            privatekey.Text = s.PrivateKey;
            password.Text = "";
            usePassword.Checked = s.UsePassword;
            usePrivateKey.Checked = !s.UsePassword;
            usePassword_CheckedChanged(null, null);
            usePrivateKey_CheckedChanged(null, null);

            disableCache.Checked = s.DisableCache;
            withoutOfflineAttribute.Checked = s.WithoutOfflineAttribute;

            drive.Text = s.Drive;
            root.Text = s.ServerRoot;         
        }

        private void SettingLoad()
        {
            settings.Load();
            settingNames.Items.Clear();
            int count = settings.Count;
            for (int i = 0; i < count; ++i)
            {
                settingNames.Items.Add(settings[i].Name);
            }
            settingNames.Items.Add("New Setting");
            settingNames.SelectedIndex = 0;
            SettingLoad(0);
        }

        private void delete_Click(object sender, EventArgs e)
        {
            settings.Delete(selectedIndex);
            settings.Save();
            SettingLoad();
        }

    }
}