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
using System.Data.OleDb;
using System.Collections;
using System.IO;

namespace ElectronicMusicTest4
{
    public partial class OwnForm : SkinMain
    {
        public OwnForm()
        {
            InitializeComponent();
            headButton1.pictureBox1.Click += new EventHandler(button_min_Click);
            headButton1.pictureBox2.Click += new EventHandler(button_close_Click);
        }

        private void OwnForm_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = true;
            DataLoad();
        }

        //加载数据库
        public void DataLoad()
        {
            try
            {
                //创建连接数据库的字符串
                //注意这里没有User ID
                string ConStr = "Provider= Microsoft.Jet.OLEDB.4.0;Data Source=..\\..\\..\\MusicDatabase\\OwnMusic.mdb; Persist Security Info=False; Jet OLEDB:Database Password=123456;";
                //创建oledbconnection对象
                OleDbConnection con = new OleDbConnection(ConStr);
                con = new OleDbConnection(ConStr);
                //打开数据库
                con.Open();

                //以下注释为从一张表中选择数据，然后加载到Listview中
                string str = "select * from 音乐";
                //加载表中所有数据
                OleDbCommand cmd = new OleDbCommand(str, con);
                OleDbDataReader oldbRed = cmd.ExecuteReader();
                cmd = new OleDbCommand(str, con);
                oldbRed = cmd.ExecuteReader();
                //不调用Read()，将会没有数据
                int i = 1;
                while (oldbRed.Read())
                {
                    ListViewItem lvi = new ListViewItem(i.ToString());
                    lvi.SubItems.Add(oldbRed[1].ToString());
                    listViewEx1.Items.Add(lvi);
                    ++i;
                }
                oldbRed.Close();
                con.Close();
                con.Dispose();
            }
            catch (Exception err)
            {
                //如果出错，显示错误信息
                MessageBox.Show(err.Message.ToString());
            }
        }

        //双击播放音乐
        bool isPlaying = false;
        public Timer timerPlay = new Timer
        {
            Interval = 100
        };
        int playTime = 0;
        public Hashtable newMusic = new Hashtable();
        private void listViewEx1_DoubleClick(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.播放按钮;
            this.pictureBox1.Size = new Size(28, 28);
            this.pictureBox1.Location = new Point(52, 448);
            if (!isPlaying)
            {
                isPlaying = true;
                timerPlay.Tick -= TimerPlay_Tick;
                int playingMusicIndex = this.listViewEx1.FocusedItem.Index;
                string musicName = this.listViewEx1.Items[playingMusicIndex].SubItems[1].Text;
                string musicPath = "..\\..\\..\\MusicDatabase\\OwnMusic\\" + musicName + ".txt";
                newMusic.Clear();
                playTime = 0;
                StreamReader sr = new StreamReader(musicPath, Encoding.UTF8);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] music = line.Split(' ');
                    newMusic.Add(int.Parse(music[0]), music[1]);
                    //MessageBox.Show(music[0]);
                    //MessageBox.Show(music[1]);
                }
                timerPlay.Tick += TimerPlay_Tick;
                timerPlay.Enabled = true;
            }
            else
            {
                timerPlay.Tick -= TimerPlay_Tick;
                timerPlay.Enabled = false;
                int playingMusicIndex = this.listViewEx1.FocusedItem.Index;
                string musicName = this.listViewEx1.Items[playingMusicIndex].SubItems[1].Text;
                string musicPath = "..\\..\\..\\MusicDatabase\\OwnMusic\\" + musicName + ".txt";
                newMusic.Clear();
                playTime = 0;
                StreamReader sr = new StreamReader(musicPath, Encoding.UTF8);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] music = line.Split(' ');
                    newMusic.Add(int.Parse(music[0]), music[1]);
                    //MessageBox.Show(music[0]);
                    //MessageBox.Show(music[1]);
                }
                timerPlay.Tick += TimerPlay_Tick;
                timerPlay.Enabled = true;
            }
        }

        public void TimerPlay_Tick(object sender, EventArgs e)
        {
            ++playTime;
            if (newMusic.ContainsKey(playTime))
            {
                string musicPath = newMusic[playTime].ToString();
                MusicPlayer(musicPath);
                //MessageBox.Show("Test");
            }

        }

        //音频播放函数
        public void MusicPlayer(string path)
        {
            //MessageBox.Show("Test1");
            //建立声音设备
            Microsoft.DirectX.DirectSound.Device dev = new Microsoft.DirectX.DirectSound.Device();
            dev.SetCooperativeLevel(this, Microsoft.DirectX.DirectSound.CooperativeLevel.Normal);
            //为声音建立二级缓冲区
            try
            {
                Microsoft.DirectX.DirectSound.SecondaryBuffer snd =
                    new Microsoft.DirectX.DirectSound.SecondaryBuffer(path, dev);
                //播放声音
                snd.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                //MessageBox.Show("Test2");
            }
            catch (Exception ex)
            {
                //label1.Text = ex.ToString();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                //this.panel2.Enabled = false;
                timerPlay.Enabled = false;
                isPlaying = false;
                pictureBox1.BackgroundImage = Properties.Resources.暂停按钮;
                this.pictureBox1.Size = new Size(25, 25);
                this.pictureBox1.Location = new Point(55, 448);
                //MessageBox.Show(playTime.ToString());
            }
            else if(playTime != 0)
            {
                //MessageBox.Show("Test");
                timerPlay.Enabled = true;
                isPlaying = true;
                pictureBox1.BackgroundImage = Properties.Resources.播放按钮;
                this.pictureBox1.Size = new Size(28, 28);
                this.pictureBox1.Location = new Point(52, 448);
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            CreateForm createForm = new CreateForm();
            createForm.AllShow();
            this.AllHide();
        }
    }
}
