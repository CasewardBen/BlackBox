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
using System.Speech.Recognition;//引用系统的Speech的识别
using System.Speech.Synthesis; //引用语音合成
using System.Threading;
using System.Media;
using System.Data.OleDb;

namespace ElectronicMusicTest4
{
    public partial class MainForm : SkinMain
    {
        public static MainForm mainForm;
        static SoundPlayer sp = new SoundPlayer();
        static SpeechSynthesizer sy = new SpeechSynthesizer();
        static bool playing = false;
        static bool running = false;
        static int playingMusicIndex = 0;
        
        public MainForm()
        {
            mainForm = this;
            InitializeComponent();
            headButton1.pictureBox1.Click += new EventHandler(button_min_Click);
            headButton1.pictureBox2.Click += new EventHandler(button_close_Click);
            pictureBox1.Click += new EventHandler(button_speech_Click);
            playButton1.pictureBox1.Click += new EventHandler(button_last_Click);
            playButton1.pictureBox2.Click += new EventHandler(button_next_Click);
            playButton1.pictureBox3.Click += new EventHandler(button_start_Click);
        }


        //???代码编写的过程中发现，有时候必须手动重新生成解决方案，如果仅仅启动仍然执行原来的代码？？？
        //加载窗体
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = true;
            DataLoad(); 
        }


        //最小化窗口
        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        //关闭窗口
        private void button_close_Click(object sender, EventArgs e)
        {
            running = false;
            Application.Exit();
        }


        //任务栏图标双击恢复窗口
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }


        //任务栏图标右击事件实现
        private void showMainWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

       
        //加载数据库
        public void DataLoad()
        {
            try
            {
                //创建连接数据库的字符串
                //注意这里没有User ID
                string ConStr = "Provider= Microsoft.Jet.OLEDB.4.0;Data Source=..\\..\\..\\MusicDatabase\\Music.mdb; Persist Security Info=False; Jet OLEDB:Database Password=123456;";
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
                while (oldbRed.Read())  
                {
                    ListViewItem lvi = new ListViewItem(oldbRed[0].ToString());
                    lvi.SubItems.Add(oldbRed[1].ToString());
                    lvi.SubItems.Add(oldbRed[2].ToString());
                    lvi.SubItems.Add(oldbRed[3].ToString());
                    listViewEx1.Items.Add(lvi);
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


        //音乐播放函数
        static void MusicPlayer(string path)
        {
            sp = new SoundPlayer(path);
            sp.PlayLooping();
        }


        //语音识别按钮单击
        private void button_speech_Click(object sender, EventArgs e)
        {
            if(running == false)
            {
                //pictureBox1.BackgroundImage = new Bitmap("D:\\C#大作业\\麦克风2.png");
                pictureBox1.BackgroundImage = Properties.Resources.麦克风2;
                Thread newThread = new Thread(BlackBoxSpeech);
                newThread.Start();
            }
            else if(running == true)
            {
                //pictureBox1.BackgroundImage = new Bitmap("D:\\C#大作业\\麦克风1.png");
                pictureBox1.BackgroundImage = Properties.Resources.麦克风1;
                running = false;
            }
        }


        //语音识别实现
        public void BlackBoxSpeech()
        {
            running = true;
            //创建中文识别器
            using (SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("zh-CN")))
            {
                //----------------
                //初始化命令词
                Choices conmmonds = new Choices();
                //添加命令词
                conmmonds.Add(new string[] { "小黑", "你好", "播放", "暂停", "下一首", "上一首" });
                //初始化命令词管理
                GrammarBuilder gBuilder = new GrammarBuilder();
                //将命令词添加到管理中
                gBuilder.Append(conmmonds);
                //实例化命令词管理
                Grammar grammar = new Grammar(gBuilder);
                //-----------------

                //创建并加载听写语法(添加命令词汇识别的比较精准)
                recognizer.LoadGrammar(grammar);
                //为语音识别事件添加处理程序。
                recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRRecongized);
                //将输入配置到语音识别器。
                recognizer.SetInputToDefaultAudioDevice();
                //启动异步，连续语音识别。
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                //MessageBox.Show("Test");
                ////保持控制台窗口打开。
                while (running)
                {
                    //    Console.ReadLine();

                }
            }
        }


        //speechrecognized事件处理
        static void recognizer_SpeechRRecongized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "小黑") sy.Speak("我在，您请说");
            else if (e.Result.Text == "播放") ifPlay();
            else if (e.Result.Text == "暂停") { sp.Stop(); sy.Speak("已暂停"); }
            else if (e.Result.Text == "下一首") mainForm.playNext();
            else if (e.Result.Text == "上一首") mainForm.playLast();
        }


        //开始播放按钮
        private void button_start_Click(object sender, EventArgs e)
        {
            ifPlay();
            
        }


        //判断播放或者暂停
        static void ifPlay()
        {
            if (playing == false)
            {
                mainForm.PlayStart();
            }
            else if (playing == true)
            {
                mainForm.PlayStop();
            }
        }


        //播放
        public void PlayStart()
        {
            //判断是否有选中歌曲
            //如果没有选中歌曲，播放第一首
            //if(!mainForm.listViewEx1.ContainsFocus)
            //{
            //    //MessageBox.Show("Test");
            //    //SelectMusic();
            //    //选中歌曲后播放
            //    listSelectedMusicPlay();
            //}
            //else
            //{
            //    //注意不要从文件的属性中复制地址，文本的格式可能有问题
            //    mainForm.playButton1.pictureBox3.BackgroundImage = new Bitmap("D:\\C#大作业\\播放按钮2.png");
            //    mainForm.playButton1.pictureBox3.Size = new Size(18, 18);
            //    mainForm.playButton1.pictureBox3.Location = new Point(12, 51);
            //    listSelectedMusicPlay();
            //    playing = true;
            //}
            //注意不要从文件的属性中复制地址，文本的格式可能有问题
            if (mainForm.playButton1.pictureBox3.InvokeRequired)
            {
                Action actionDelegate1 = () => { mainForm.playButton1.pictureBox3.BackgroundImage = Properties.Resources.播放按钮; };
                //mainForm.playButton1.pictureBox3.Invoke(actionDelegate1, "D:\\C#大作业\\播放按钮2.png");
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate1);
                Action<int, int> actionDelegate2 = (x, y) => { mainForm.playButton1.pictureBox3.Size = new Size(x, y); };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate2, 18, 18);
                Action<int, int> actionDelegate3 = (x, y) => { mainForm.playButton1.pictureBox3.Location = new Point(x, y); };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate3, 12, 51);
            }
            else
            {
                //mainForm.playButton1.pictureBox3.BackgroundImage = new Bitmap("D:\\C#大作业\\播放按钮2.png");
                mainForm.playButton1.pictureBox3.BackgroundImage = Properties.Resources.播放按钮;
                mainForm.playButton1.pictureBox3.Size = new Size(18, 18);
                mainForm.playButton1.pictureBox3.Location = new Point(12, 51);
            }
            //mainForm.playButton1.pictureBox3.BackgroundImage = new Bitmap("D:\\C#大作业\\播放按钮2.png");
            //mainForm.playButton1.pictureBox3.Size = new Size(18, 18);
            //mainForm.playButton1.pictureBox3.Location = new Point(12, 51);
            listSelectedMusicPlay();
            playing = true;
        }


        //选中歌曲
        public void SelectMusic()
        {
            if (mainForm.listViewEx1.InvokeRequired)
            {
                Action<int> actionDelegate1 = (x) => { mainForm.listViewEx1.Items[x].Selected = true; };
                mainForm.listViewEx1.Invoke(actionDelegate1, playingMusicIndex);
                Action<int> actionDelegate2 = (x) => { mainForm.listViewEx1.Items[x].EnsureVisible(); };
                mainForm.listViewEx1.Invoke(actionDelegate2, playingMusicIndex);
                Action<int> actionDelegate3 = (x) => { mainForm.listViewEx1.Items[x].Focused = true; };
                mainForm.listViewEx1.Invoke(actionDelegate3, playingMusicIndex);
                Action<int> actionDelegate4 = (x) => { mainForm.listViewEx1.Select(); };
                mainForm.listViewEx1.Invoke(actionDelegate4, playingMusicIndex);
            }
            else
            {
                mainForm.listViewEx1.Items[playingMusicIndex].Selected = true;
                mainForm.listViewEx1.Items[playingMusicIndex].EnsureVisible();
                mainForm.listViewEx1.Items[playingMusicIndex].Focused = true;
                mainForm.listViewEx1.Select();
            }
        }


        //取消选中歌曲
        public void CelSelectMusic(int id)
        {
            if (mainForm.listViewEx1.InvokeRequired)
            {
                Action<int> actionDelegate1 = (x) => { mainForm.listViewEx1.Items[id].Selected = false; };
                mainForm.listViewEx1.Invoke(actionDelegate1, id);
                Action<int> actionDelegate2 = (x) => { mainForm.listViewEx1.Items[id].Focused = false; };
                mainForm.listViewEx1.Invoke(actionDelegate2, id);
                
            }
            else
            {
                mainForm.listViewEx1.Items[id].Selected = false;
                mainForm.listViewEx1.Items[id].Focused = false;
            }
            
        }


        //暂停
        public void PlayStop()
        {
            if (mainForm.playButton1.pictureBox3.InvokeRequired)
            {
                //Action<string> actionDelegate1 = (x) => { mainForm.playButton1.pictureBox3.BackgroundImage = new Bitmap(x); };
                Action actionDelegate1 = () => { mainForm.playButton1.pictureBox3.BackgroundImage = Properties.Resources.暂停按钮; };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate1);
                Action<int, int> actionDelegate2 = (x, y) => { mainForm.playButton1.pictureBox3.Size = new Size(x, y); };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate2, 18, 20);
                Action<int, int> actionDelegate3 = (x, y) => { mainForm.playButton1.pictureBox3.Location = new Point(x, y); };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate3, 14, 48);
            }
            else
            {
                mainForm.playButton1.pictureBox3.BackgroundImage = Properties.Resources.暂停按钮;
                mainForm.playButton1.pictureBox3.Size = new Size(18, 20);
                mainForm.playButton1.pictureBox3.Location = new Point(14, 48);
            }
            //mainForm.playButton1.pictureBox3.BackgroundImage = new Bitmap("D:\\C#大作业\\暂停按钮.png");
            //mainForm.playButton1.pictureBox3.Size = new Size(18, 20);
            //mainForm.playButton1.pictureBox3.Location = new Point(14, 48);
            sp.Stop();
            playing = false;
        }


        //播放上一首
        private void button_last_Click(object sender, EventArgs e)
        {
            playLast();
        }

        public void playLast()
        {
            playing = true;
            if (mainForm.playButton1.pictureBox3.InvokeRequired)
            {
                //Action<string> actionDelegate1 = (x) => { mainForm.playButton1.pictureBox3.BackgroundImage = new Bitmap(x); };
                Action actionDelegate1 = () => { mainForm.playButton1.pictureBox3.BackgroundImage = Properties.Resources.播放按钮; };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate1);
                Action<int, int> actionDelegate2 = (x, y) => { mainForm.playButton1.pictureBox3.Size = new Size(x, y); };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate2, 18, 18);
                Action<int, int> actionDelegate3 = (x, y) => { mainForm.playButton1.pictureBox3.Location = new Point(x, y); };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate3, 12, 51);
            }
            else
            {
                mainForm.playButton1.pictureBox3.BackgroundImage = Properties.Resources.播放按钮;
                mainForm.playButton1.pictureBox3.Size = new Size(18, 18);
                mainForm.playButton1.pictureBox3.Location = new Point(12, 51);
            }
            int id = playingMusicIndex;
            if (id > 0)
            {
                playingMusicIndex = id - 1;
                SelectMusic();
            }
            else
            {
                playingMusicIndex = mainForm.listViewEx1.Items.Count - 1;
                SelectMusic();
            }
            CelSelectMusic(id);
            listSelectedMusicPlay();
        }


        //播放下一首
        private void button_next_Click(object sender, EventArgs e)
        {
            playNext();
        }
        
        public void playNext()
        {
            playing = true;
            if (mainForm.playButton1.pictureBox3.InvokeRequired)
            {
                Action actionDelegate1 = () => { mainForm.playButton1.pictureBox3.BackgroundImage = Properties.Resources.播放按钮; };
                //mainForm.playButton1.pictureBox3.Invoke(actionDelegate1, "D:\\C#大作业\\播放按钮2.png");
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate1);
                Action <int, int> actionDelegate2 = (x, y) => { mainForm.playButton1.pictureBox3.Size = new Size(x, y); };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate2, 18, 18);
                Action<int, int> actionDelegate3 = (x, y) => { mainForm.playButton1.pictureBox3.Location = new Point(x, y); };
                mainForm.playButton1.pictureBox3.Invoke(actionDelegate3, 12, 51);
            }
            else
            {
                mainForm.playButton1.pictureBox3.BackgroundImage = Properties.Resources.播放按钮;
                mainForm.playButton1.pictureBox3.Size = new Size(18, 18);
                mainForm.playButton1.pictureBox3.Location = new Point(12, 51);
            }
            int id = playingMusicIndex;
            if (id < mainForm.listViewEx1.Items.Count - 1)
            {
                playingMusicIndex = id + 1;
                SelectMusic();
            }
            else
            {
                playingMusicIndex = 0;
                SelectMusic();
            }
            CelSelectMusic(id);
            listSelectedMusicPlay();
        }

        //双击播放音乐
        private void listViewEx1_DoubleClick(object sender, EventArgs e)
        {
            int id = playingMusicIndex;
            playingMusicIndex = mainForm.listViewEx1.FocusedItem.Index;
            CelSelectMusic(id);
            SelectMusic();
            PlayStart();
        }


        //列表选中的音乐播放
        public delegate void MyDelegate(int s, int x);
        static void listSelectedMusicPlay()
        {
            string musicPath = "";
            string musicName = "";
            if (mainForm.listViewEx1.InvokeRequired)
            {
                //MessageBox.Show("Test");
                MyDelegate actionDelegate = (int s, int x) => { musicName = mainForm.listViewEx1.Items[s].SubItems[x].Text; };
                mainForm.listViewEx1.Invoke(actionDelegate, playingMusicIndex, 1);
                //mainForm.listViewEx1.Invoke(actionDelegate, playingMusicIndex, 1);
                //musicName = actionDelegate(playingMusicIndex, 1);
            }
            else
            {
                musicName = mainForm.listViewEx1.Items[playingMusicIndex].SubItems[1].Text;
            }
            //string musicName = mainForm.listViewEx1.Items[playingMusicIndex].SubItems[1].Text;
            string ConStr = "Provider= Microsoft.Jet.OLEDB.4.0;Data Source=..\\..\\..\\MusicDatabase\\Music.mdb; Persist Security Info=False; Jet OLEDB:Database Password=123456;";
            OleDbConnection con = new OleDbConnection(ConStr);
            con = new OleDbConnection(ConStr);
            con.Open();
            string sql = "select Mpath from 音乐 where Mname='" + musicName + "'";
            OleDbCommand cmd = new OleDbCommand(sql, con);
            OleDbDataReader oldbRed = cmd.ExecuteReader();
            while (oldbRed.Read())
            {
                musicPath = oldbRed[0].ToString();
                
            }
            MusicPlayer(@musicPath);
            oldbRed.Close();
            con.Close();
            con.Dispose();
        }


        //设置listviewEx列宽不可改变
        private void listViewEx1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listViewEx1.Columns[e.ColumnIndex].Width;
        }


        //设置能够保持播放歌曲行和鼠标焦点行同时保持选中状态
        private void listViewEx1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                mainForm.listViewEx1.Items[playingMusicIndex].Selected = true;
            }
            catch(Exception ex)
            {
                //防止mainForm.listViewEx1.Items[playingMusicIndex].Selected数组序号越界
            }
        }


        //设置鼠标移出listViewEx时未播放的歌曲取消Selected状态
        private void listViewEx1_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                mainForm.listViewEx1.FocusedItem.Selected = false;
            }
            catch(Exception ex)
            {
                //防止mainForm.listViewEx1.FocusedItem.Selected对象实例不存在
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            CreateForm createForm = new CreateForm();
            createForm.AllShow();
            this.AllHide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            BeginForm beginForm = new BeginForm();
            beginForm.AllShow();
            this.AllHide();
        }
    }
}
