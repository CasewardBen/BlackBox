namespace ElectronicMusicTest4
{
    partial class BeginForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BeginForm));
            this.button_min = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMainWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.headButton1 = new ElectronicMusicTest4.HeadButton();
            this.startButton1 = new ElectronicMusicTest4.StartButton();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_min
            // 
            this.button_min.BackColor = System.Drawing.Color.Transparent;
            this.button_min.FlatAppearance.BorderSize = 0;
            this.button_min.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_min.Location = new System.Drawing.Point(270, 4);
            this.button_min.Name = "button_min";
            this.button_min.Size = new System.Drawing.Size(20, 20);
            this.button_min.TabIndex = 3;
            this.button_min.TabStop = false;
            this.button_min.UseVisualStyleBackColor = false;
            this.button_min.Visible = false;
            this.button_min.Click += new System.EventHandler(this.button_min_Click);
            // 
            // button_close
            // 
            this.button_close.BackColor = System.Drawing.Color.Transparent;
            this.button_close.FlatAppearance.BorderSize = 0;
            this.button_close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_close.Location = new System.Drawing.Point(310, 7);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(20, 20);
            this.button_close.TabIndex = 4;
            this.button_close.TabStop = false;
            this.button_close.UseVisualStyleBackColor = false;
            this.button_close.Visible = false;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMainWindowToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(192, 26);
            // 
            // showMainWindowToolStripMenuItem
            // 
            this.showMainWindowToolStripMenuItem.Name = "showMainWindowToolStripMenuItem";
            this.showMainWindowToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.showMainWindowToolStripMenuItem.Text = "Show Main Window";
            this.showMainWindowToolStripMenuItem.Click += new System.EventHandler(this.showMainWindowToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "MBox";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // headButton1
            // 
            this.headButton1.BackColor = System.Drawing.Color.Transparent;
            this.headButton1.Location = new System.Drawing.Point(271, 7);
            this.headButton1.Margin = new System.Windows.Forms.Padding(2);
            this.headButton1.Name = "headButton1";
            this.headButton1.Size = new System.Drawing.Size(58, 20);
            this.headButton1.TabIndex = 5;
            // 
            // startButton1
            // 
            this.startButton1.BackColor = System.Drawing.Color.Transparent;
            this.startButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("startButton1.BackgroundImage")));
            this.startButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.startButton1.Location = new System.Drawing.Point(137, 187);
            this.startButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.startButton1.Name = "startButton1";
            this.startButton1.Size = new System.Drawing.Size(66, 58);
            this.startButton1.TabIndex = 1;
            this.startButton1.Click += new System.EventHandler(this.startButton1_Click);
            // 
            // BeginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(340, 490);
            this.Controls.Add(this.headButton1);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_min);
            this.Controls.Add(this.startButton1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainPosition = new System.Drawing.Point(42, 55);
            this.Name = "BeginForm";
            this.ShowInTaskbar = true;
            this.SkinBack = ((System.Drawing.Bitmap)(resources.GetObject("$this.SkinBack")));
            this.SkinShowInTaskbar = false;
            this.SkinSize = new System.Drawing.Size(400, 580);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private StartButton startButton1;
        private System.Windows.Forms.Button button_min;
        private System.Windows.Forms.Button button_close;
        private HeadButton headButton1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showMainWindowToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

