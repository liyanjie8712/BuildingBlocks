using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Liyanjie.DesktopWebHost
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            var favicon = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "favicon.ico"));
            if (File.Exists(favicon))
            {
                var icon = new System.Drawing.Icon(favicon);
                this.Icon = icon;
                this.NotifyIcon.Icon = icon;
            }
            var appname = ConfigurationManager.AppSettings["Appname"];
            if (!string.IsNullOrEmpty(appname))
            {
                this.Text = appname;
                this.NotifyIcon.Text = appname;
            }

            this.Visible = false;
            this.FormClosing += Form_FormClosing;
            this.LogShowing += Form_LogShowing;

            WebHostManager.StartWebHost();
            foreach (var url in WebHostManager.GetUrls().Reverse())
            {
                var item = new ToolStripMenuItem
                {
                    Name = url,
                    Text = url,
                    Size = new System.Drawing.Size(322, 38),
                    Tag = url,
                };
                item.Click += ToolStripMenuItem_Open_Click;
                this.ContextMenuStrip_NotifyIcon.Items.Insert(0, item);
            }
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Visible = false;

                return;
            }
        }
        private void Form_LogShowing(object sender, Logging.LogMessage e)
        {
            this.TextBox.Text += e.Message;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void ToolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                Process.Start("explorer", item.Tag.ToString());
        }
        private void ToolStripMenuItem_Restart_Click(object sender, EventArgs e)
        {
            WebHostManager.CloseWebHost();
            WebHostManager.StartWebHost();
        }
        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            WebHostManager.CloseWebHost();
            Application.Exit();
        }

        private event EventHandler<Logging.LogMessage> LogShowing;

        internal void ShowLog(Logging.LogMessage log)
        {
            LogShowing?.Invoke(this, log);
        }
    }
}
