using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ServiceProcess;
using System.Diagnostics;

namespace DokanSSHFS
{
    public partial class B2BRouter : Form
    {
        ServiceController b2bservice;
        String srvstatus = "";

        public Settings settings = new Settings();

        public B2BRouter()
        {
            InitializeComponent();
            this.textBox2.Text = this.ReadPublicKey();
            b2bservice = new ServiceController();
            b2bservice.ServiceName = "B2brouterService";
            Redraw();
        }

        public void start_Click(object sender, EventArgs e)
        {
            String message = null;
            if (user.Text == "")
            {
                // do not start service if user is blank!
                message = "Please enter your username.";
            }
            else
            {
                this.progressBar1.Visible = true;
                settingSave();
                srvstatus = b2bservice.Status.ToString();
                if (srvstatus == "Stopped")
                {
                    b2bservice.Start();
                    int i = 0;
                    this.progressBar1.Value = i * 10;
                    while (srvstatus != "Running" && i < 10)
                    {
                        this.progressBar1.Value = i * 10;
                        b2bservice.Refresh();
                        srvstatus = b2bservice.Status.ToString();
                        System.Threading.Thread.Sleep(1000);
                        i += 1;
                    }
                    if (i >= 10)
                    {
                        message = "Failed to connect.";
                    } 
                    else 
                    {
                        // don't draw connected in red...
                        //message = "Connected.";
                    }
                }
                else
                {
                    message = "Service is not stopped.";
                }
            }
            Redraw(message);
            //Redraw();
            this.progressBar1.Visible = false;
        }

        public void stop_Click(object sender, EventArgs e)
        {
            this.progressBar1.Visible = true;
            srvstatus = b2bservice.Status.ToString();
            if (srvstatus == "Running")
            {
                b2bservice.Stop();
                int i = 0;
                while (srvstatus != "Stopped" && i < 10)
                {
                    this.progressBar1.Value = i * 10;
                    b2bservice.Refresh();
                    srvstatus = b2bservice.Status.ToString();
                    System.Threading.Thread.Sleep(1000);
                    i += 1;
                }
            }
            else if ( srvstatus == "StartPending" ){
                KillProcess();
                b2bservice.Refresh();
                srvstatus = b2bservice.Status.ToString();
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                // ???
            }
            Redraw();
            this.progressBar1.Visible = false;
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

        public void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void SettingLoad()
        {
            settings.Load();
            Setting s = settings[0];

            user.Text = s.User;
            drive.Text = s.Drive;
        }

        private void Redraw(String message = null)
        {
            String default_message = null;
            try {
                srvstatus = b2bservice.Status.ToString();
            } catch {}
            if (srvstatus == "Running")
            {
                this.start.Enabled = false;
                this.stop.Enabled = true;
                this.user.Enabled = false;
                this.drive.Enabled = false;
                this.label5.ForeColor = System.Drawing.Color.ForestGreen;
                default_message = "Service is currently running";
            }
            else if (srvstatus == "Stopped")
            {
                this.start.Enabled = true;
                this.stop.Enabled = false;
                this.user.Enabled = true;
                this.drive.Enabled = true;
                this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
                default_message = "Service is currently stopped";
            }
            else if (srvstatus == "StartPending")
            {
                this.start.Enabled = false;
                this.stop.Enabled = true;
                this.user.Enabled = false;
                this.drive.Enabled = false;
                this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
                default_message = "Service is trying to connect";
            }
            else
            {
                this.start.Enabled = false;
                this.stop.Enabled = false;
                this.user.Enabled = false;
                this.drive.Enabled = false;
                this.label5.ForeColor = System.Drawing.Color.Firebrick;
                default_message = "Service is not correctly installed (" + srvstatus + ")";
            }
            if ( message != null ) {
                this.label5.ForeColor = System.Drawing.Color.Firebrick;
                this.label5.Text = message;
            } else {
                this.label5.Text = default_message;
            }
        }

        private String ReadPublicKey()
        {
            String pubkeyloc = ProgramFilesx86() + "\\b2brouter\\b2brouter.key.pub";
            TextReader tr = new StreamReader(pubkeyloc);
            String pubkey = tr.ReadLine();
            tr.Close();
            return pubkey;
        }

        public static String ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }
            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        public static void KillProcess()
        {
            foreach (Process p in System.Diagnostics.Process.GetProcessesByName("B2brouterService"))
            {
                try
                {
                    p.Kill();
                    p.WaitForExit(); // possibly with a timeout
                }
                catch (Win32Exception winException)
                {
                    // process was terminating or can't be terminated - deal with it
                }
                catch (InvalidOperationException invalidException)
                {
                    // process has already exited - might be able to let this one go
                }
            }
        }
    }
}
