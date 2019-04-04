using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;

namespace ElectronicMusicTest4
{
    public partial class BeginForm : SkinMain
    {
        public BeginForm()
        {
            InitializeComponent();
            headButton1.pictureBox1.Click += new EventHandler(button_min_Click);
            headButton1.pictureBox2.Click += new EventHandler(button_close_Click);

        }

        private void startButton1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Test");
            notifyIcon1.Visible = false;
            MainForm mainForm = new MainForm();
            mainForm.AllShow();
            this.AllHide();
        }

        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showMainWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

    }
}
