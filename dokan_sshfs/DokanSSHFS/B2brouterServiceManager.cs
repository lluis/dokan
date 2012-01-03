﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ServiceProcess;

namespace DokanSSHFS
{
    public partial class B2brouterServiceManager : Form
    {
        ServiceController b2bservice;
        String srvstatus = "";

        public Settings settings = new Settings();

        public B2brouterServiceManager()
        {
            InitializeComponent();
            this.textBox2.Text = this.ReadPublicKey();
            b2bservice = new ServiceController();
            b2bservice.ServiceName = "B2brouterService";
            Redraw();
        }

        public void start_Click(object sender, EventArgs e)
        {
            settingSave();
            srvstatus = b2bservice.Status.ToString();
            if (srvstatus == "Stopped") {
                b2bservice.Start();
                int i = 0;
                while (srvstatus != "Running" && i < 10)
                {
                    b2bservice.Refresh();
                    srvstatus = b2bservice.Status.ToString();
                    System.Threading.Thread.Sleep(1000);
                    i += 1;
                }
            } else {
                // ???
            }
            Redraw();
        }

        public void stop_Click(object sender, EventArgs e)
        {
            srvstatus = b2bservice.Status.ToString();
            if (srvstatus == "Running")
            {
                b2bservice.Stop();
                int i = 0;
                while (srvstatus != "Stopped" && i < 10)
                {
                    b2bservice.Refresh();
                    srvstatus = b2bservice.Status.ToString();
                    System.Threading.Thread.Sleep(1000);
                    i += 1;
                }
            }
            else
            {
                // ???
            }
            Redraw();
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

        private void Redraw()
        {
            try
            {
                srvstatus = b2bservice.Status.ToString();
            }
            catch
            {
                //MessageBox.Show("B2BRouter service is not installed!", "Error");
            }
            if (srvstatus == "Running")
            {
                this.start.Enabled = false;
                this.stop.Enabled = true;
                this.user.Enabled = false;
                this.drive.Enabled = false;
                this.label5.Text = "Service is currently running";
            }
            else if (srvstatus == "Stopped")
            {
                this.start.Enabled = true;
                this.stop.Enabled = false;
                this.user.Enabled = true;
                this.drive.Enabled = true;
                this.label5.Text = "Service is currently stopped";
            }
            else
            {
                this.start.Enabled = false;
                this.stop.Enabled = false;
                this.user.Enabled = false;
                this.drive.Enabled = false;
                this.label5.Text = "Service is not correctly installed";
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

    }
}
