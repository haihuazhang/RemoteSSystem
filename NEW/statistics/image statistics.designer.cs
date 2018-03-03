namespace RemoteSystem
{
    partial class Imagestatistics
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.单波段处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.统计值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.极差纹理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.单波段处理ToolStripMenuItem,
            this.统计值ToolStripMenuItem,
            this.极差纹理ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(330, 25);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 单波段处理ToolStripMenuItem
            // 
            this.单波段处理ToolStripMenuItem.Name = "单波段处理ToolStripMenuItem";
            this.单波段处理ToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.单波段处理ToolStripMenuItem.Text = "单波段处理";
            this.单波段处理ToolStripMenuItem.Click += new System.EventHandler(this.单波段处理ToolStripMenuItem_Click);
            // 
            // 统计值ToolStripMenuItem
            // 
            this.统计值ToolStripMenuItem.Name = "统计值ToolStripMenuItem";
            this.统计值ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.统计值ToolStripMenuItem.Text = "统计值";
            this.统计值ToolStripMenuItem.Click += new System.EventHandler(this.统计值ToolStripMenuItem_Click);
            // 
            // 极差纹理ToolStripMenuItem
            // 
            this.极差纹理ToolStripMenuItem.Name = "极差纹理ToolStripMenuItem";
            this.极差纹理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.极差纹理ToolStripMenuItem.Text = "极差纹理";
            this.极差纹理ToolStripMenuItem.Click += new System.EventHandler(this.极差纹理ToolStripMenuItem_Click);
            // 
            // Imagestatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 300);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Imagestatistics";
            this.Text = "Statistics ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 单波段处理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 统计值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 极差纹理ToolStripMenuItem;
    }
}

