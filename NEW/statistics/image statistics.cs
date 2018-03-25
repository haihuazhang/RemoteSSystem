using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteSystem
{
    public partial class Imagestatistics : Form
    {
        public Imagestatistics()
        {
            InitializeComponent();
        }
        public string FileName;
        //string hdrPath = "";
        //string dataPath = "";
        public int[,] BandsData ;
        public double[,] BandsDataD;
        public int[,] showdata;
        public int ColumnCounts, LineCounts, bands;
        public string Interleave;
        public int totalnum;
        public int flag=-1;
        //public byte[] bits ;

        /// <summary>
        /// 直方图|累计直方图|共生矩阵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 单波段处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = 0;
            
            singleband sgb = new singleband(ColumnCounts, LineCounts, bands);
            sgb.BandsDataD = this.BandsDataD;
            sgb.BandsData = this.BandsData;
            sgb.showdata = this.showdata;
            sgb.ColumnCounts = this.ColumnCounts;
            sgb.LineCounts = this.LineCounts;
            sgb.bands = this.bands;
            sgb.Show();
        }
        /// <summary>
        /// 波段统计值，联合直方图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 统计值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = 1;
            statistics f4 = new statistics(ColumnCounts, LineCounts, bands);
            f4.showdata = this.showdata;
            f4.BandsDataD = this.BandsDataD;
            f4.ColumnCounts = this.ColumnCounts;
            f4.LineCounts = this.LineCounts;
            f4.bands = this.bands;
            //f4.union = this.union;
            f4.Show();
        }
        /// <summary>
        /// 极差纹理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 极差纹理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = 2;
            RangeTexture rt = new RangeTexture(this.ColumnCounts, this.LineCounts, bands);
            rt.bandints = this.BandsData;
            rt.ColumnCounts = this.ColumnCounts;
            rt.LineCounts = this.LineCounts;
            rt.bands = this.bands;
            rt.Show();
        }

        
    }
}
