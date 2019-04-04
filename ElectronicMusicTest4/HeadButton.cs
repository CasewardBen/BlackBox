using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicMusicTest4
{
    public partial class HeadButton : UserControl
    {
        public HeadButton()
        {
            InitializeComponent();
        }

        public int formState = 0;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            formState = 1;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            formState = 2;
        }
    }
}
