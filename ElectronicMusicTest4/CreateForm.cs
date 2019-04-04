using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using Timer = System.Windows.Forms.Timer;
using System.Collections;
using System.Data.OleDb;
using Microsoft.VisualBasic;

namespace ElectronicMusicTest4
{
    public partial class CreateForm : SkinMain
    {
        public CreateForm()
        {
            InitializeComponent();
            headButton1.pictureBox1.Click += new EventHandler(button_min_Click);
            headButton1.pictureBox2.Click += new EventHandler(button_close_Click);
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

        //钢琴按键
        private void CreateForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A: MusicPlayer("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoC.wav");
                    ifRecord("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoC.wav"); break;
                case Keys.S: MusicPlayer("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoD.wav");
                    ifRecord("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoD.wav");break;
                case Keys.D: MusicPlayer("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoE.wav");
                    ifRecord("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoE.wav");break;
                case Keys.F: MusicPlayer("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoF.wav");
                    ifRecord("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoF.wav");break;
                case Keys.J: MusicPlayer("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoG.wav");
                    ifRecord("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoG.wav"); break;
                case Keys.K: MusicPlayer("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoA.wav");
                    ifRecord("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoA.wav");break;
                case Keys.L: MusicPlayer("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoB.wav");
                    ifRecord("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoB.wav"); break;
                case Keys.P: MusicPlayer("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoC2.wav");
                    ifRecord("..\\..\\..\\MusicDatabase\\SoundFile\\钢琴键音\\pianoC2.wav"); break;
            }
        }

        public void ifRecord(string thePath)
        {
            try
            {
                if (timerCreate.Enabled)
                {
                    newMusic.Add(musicTime, thePath);
                }
            }
            catch(Exception e)
            {

            }
            
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (!isCreating)
            {
                MessageBox.Show("开始录制！");
                button6.Text = "结束";
                musicTime = 0;
                newMusic.Clear();
                timerCreate.Enabled = true;
                timerCreate.Tick += TimerCreate_Tick;
                timerCreate.Start();
                isCreating = true;
            }
            else
            {
                timerCreate.Enabled = false;
                MessageBox.Show("录制结束！");
                string musicName = Interaction.InputBox("音乐名称", "音乐命名", "请输入你的音乐名称", -1, -1);
                string musicPath = "..\\..\\..\\MusicDatabase\\OwnMusic\\" + musicName + ".txt";
                string ConStr = "Provider= Microsoft.Jet.OLEDB.4.0;Data Source=..\\..\\..\\MusicDatabase\\OwnMusic.mdb; Persist Security Info=False; Jet OLEDB:Database Password=123456;";
                OleDbConnection con = new OleDbConnection(ConStr);
                con = new OleDbConnection(ConStr);
                con.Open();
                string sql = "select * from 音乐 where Mname='" + musicName + "'";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                OleDbDataReader oldbRed = cmd.ExecuteReader();
                bool existMusic = false;
                while (oldbRed.Read())
                {
                    existMusic = true;
                }
                oldbRed.Close();
                con.Close();
                con.Dispose();
                if (existMusic)
                {
                    MessageBox.Show("音乐名已存在，请输入新的音乐名！");
                    musicName = Interaction.InputBox("音乐名称", "音乐命名", "请输入你的音乐名称", -1, -1);
                }
                else
                {
                    string ConStr1 = "Provider= Microsoft.Jet.OLEDB.4.0;Data Source=..\\..\\..\\MusicDatabase\\OwnMusic.mdb; Persist Security Info=False; Jet OLEDB:Database Password=123456;";
                    OleDbConnection con1 = new OleDbConnection(ConStr1);
                    con1 = new OleDbConnection(ConStr1);
                    con1.Open();
                    string sql1 = "insert into 音乐(Mname)values('" + musicName + "')";
                    OleDbCommand cmd1 = new OleDbCommand(sql1, con1);
                    cmd1.ExecuteNonQuery();
                    con1.Close();
                    con1.Dispose();
                    FileStream fs = new FileStream(musicPath, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (DictionaryEntry de in newMusic)
                    {
                        sw.Write(de.Key);
                        sw.Write(' ');
                        sw.WriteLine(de.Value);
                    }
                    sw.Flush();
                    sw.Close();
                    sw.Close();
                    isCreating = false;
                    newMusic.Clear();
                }

            }
        }

        public bool isCreating = false;
        public Timer timerCreate = new Timer
        {
            Interval = 100
        };
        public int musicTime = 0;
        public Hashtable newMusic = new Hashtable();
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }
        public void TimerCreate_Tick(object sender,EventArgs e)
        {
            ++musicTime;
        }

        //public Timer timerPlay = new Timer
        //{
        //    Interval = 100
        //};
        //int playTime = 0;
        //private void button8_Click(object sender, EventArgs e)
        //{
        //    newMusic.Clear();
        //    button8.Enabled = false;
        //    playTime = 0;
        //    StreamReader sr = new StreamReader("newMusic.txt", Encoding.UTF8);
        //    string line;
        //    while((line = sr.ReadLine()) != null)
        //    {
        //        string[] music = line.Split(' ');
        //        newMusic.Add(int.Parse(music[0]),music[1]);
        //        MessageBox.Show(music[0]);
        //        MessageBox.Show(music[1]);
        //    }
        //    timerPlay.Tick += TimerPlay_Tick;
        //    timerPlay.Enabled = true;
        //    MessageBox.Show("Test");
        //}
        //public void  TimerPlay_Tick(object sender,EventArgs e)
        //{
        //    ++playTime;
        //    if (newMusic.ContainsKey(playTime))
        //    {
        //        string musicPath = newMusic[playTime].ToString();
        //        MusicPlayer(musicPath);
        //        MessageBox.Show("Test");
        //    }

        //}

        private void button9_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            OwnForm ownForm = new OwnForm();
            ownForm.AllShow();
            this.AllHide();
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            MainForm mainForm = new MainForm();
            mainForm.AllShow();
            this.AllHide();
        }
    }
}
