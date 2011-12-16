using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DokanSSHFS
{
    partial class B2brouter
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.label3 = new System.Windows.Forms.Label();
            this.user = new System.Windows.Forms.TextBox();
            this.connect = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.config = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.drive = new System.Windows.Forms.ComboBox();
			this.publicKey = new System.Windows.Forms.TextBox();
			this.txt1 = new System.Windows.Forms.TextBox();
			this.txt2 = new System.Windows.Forms.TextBox();
			this.banner = "B2BRouter";
			this.text1 = "Welcome to the Windows client of B2BRouter. " +
			 	"Please copy & paste your public key into b2brouter.net form";
			this.text2 = "If you don't have a user, please contact us at comercial@invinet.org";
            this.notifyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(198, 304);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(75, 23);
            this.connect.TabIndex = 15;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(279, 304);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 16;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.notifyMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "B2BRouter";
            this.notifyIcon1.Visible = true;
            // 
            // notifyMenu
            // 
            this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exit,
            this.config});
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
            // config
            // 
            this.config.Name = "config";
            this.config.Size = new System.Drawing.Size(90, 22);
            this.config.Text = "Config";
            this.config.Click += new System.EventHandler(this.config_Click);
            // 
            // label3 user
            // 
            this.label3.Location = new System.Drawing.Point(5, 195);
			this.label3.AutoSize = true;
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "User";
            // 
            // user
            // 
            this.user.Location = new System.Drawing.Point(41, 193);
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(173, 19);
            this.user.TabIndex = 5;
			this.user.Text = "b2b_";
			// 
            // label5 drive
            // 
            this.label5.Location = new System.Drawing.Point(5, 216);
			this.label5.AutoSize = true;
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "Drive";
            // 
            // drive
            // 
			this.drive.Location = new System.Drawing.Point(41, 213);
            this.drive.FormattingEnabled = true;
            this.drive.Items.AddRange(new object[] {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
			"N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"});
            this.drive.Name = "drive";
            this.drive.Size = new System.Drawing.Size(53, 20);
            this.drive.TabIndex = 24;
            this.drive.Text = "N";
			//
			// Public Key
			//
			this.publicKey.Location = new System.Drawing.Point(5,60);
			this.publicKey.Size = Size = new System.Drawing.Size(350, 100);
			this.publicKey.Text = this.ReadPublicKey();
			this.publicKey.ReadOnly = true;
			this.publicKey.Multiline = true;
			this.publicKey.ScrollBars = ScrollBars.Vertical;
			/// txt1
			this.txt1.Location = new System.Drawing.Point(5,30);
			this.txt1.Size = Size = new System.Drawing.Size(350, 100);
			this.txt1.Text = text1;
			this.txt1.ReadOnly = true;
			this.txt1.Multiline = true;
			this.txt1.ScrollBars = ScrollBars.None;
			this.txt1.BorderStyle = BorderStyle.None;
			/// txt2
			this.txt2.Location = new System.Drawing.Point(5,175);
			this.txt2.Size = Size = new System.Drawing.Size(350, 100);
			this.txt2.Text = text2;
			this.txt2.ReadOnly = true;
			this.txt2.Multiline = true;
			this.txt2.ScrollBars = ScrollBars.None;
			this.txt2.BorderStyle = BorderStyle.None;
			//
			// Banner
			//
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintBanner);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 335);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.user);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.drive);
			this.Controls.Add(this.publicKey);
			this.Controls.Add(this.txt1);
			this.Controls.Add(this.txt2);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.connect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "B2BRouter Setting";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.notifyMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		
		protected void PaintBanner(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.DrawString(banner, new Font("times New Roman", 18), Brushes.Black, 18, 3);
		}
		
		private String ReadPublicKey()
		{
			String pubkeyloc = Environment.ExpandEnvironmentVariables("%ProgramFiles%\\b2brouter\\b2brouter.key.pub");
			TextReader tr = new StreamReader(pubkeyloc);
			String pubkey = tr.ReadLine();
			tr.Close();
			return pubkey;
		}
		
        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox user;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip notifyMenu;
        private System.Windows.Forms.ToolStripMenuItem exit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem config;
        private System.Windows.Forms.ComboBox drive;
		private System.Windows.Forms.TextBox publicKey;
		private System.Windows.Forms.TextBox txt1;
		private System.Windows.Forms.TextBox txt2;
		private String banner;
		private String text1;
		private String text2;
    }
}