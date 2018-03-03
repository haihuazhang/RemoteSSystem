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
    /// <summary>
    /// 极差纹理
    /// </summary>
    public partial class RangeTexture : Form
    {
        public RangeTexture(int ColumnCounts,int LineCounts,int bands)
        {
            InitializeComponent();
            bandints = new int[bands, ColumnCounts*LineCounts];
            meanrange = new double[ColumnCounts * LineCounts];
        }
        public int[,] bandints;
        public int ColumnCounts, LineCounts, bands;
        public double[] meanrange;
        public int[,] orglpixel ;
        /// <summary>
        /// 开始计算极差纹理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            orglpixel =new int[this.LineCounts,this.ColumnCounts];
            int i = Convert.ToInt32(comboBox1.Text.Substring(4)) - 1;
            int meansize;int max,min;
            if (radioButton1.Checked)
            {
                meansize = 3;
            }
            else if(radioButton2.Checked)
            {
                meansize =5;
            }
            else
            {
                meansize = 0;
                MessageBox.Show("请输入窗口大小!");
                return;
            }
            for (int j = 0; j < LineCounts; j++)
            {
                for (int k = 0; k < ColumnCounts; k++)
                {
                    orglpixel[j, k] = bandints[i, k + j * ColumnCounts];
                }
            }
            for (int j = 0; j < LineCounts; j++)
            {
                for (int k = 0; k < ColumnCounts; k++)
                {
                    max=min=orglpixel[j,k];
                    for (int p = -1; p < meansize-1; p++)
                    {
                        for (int q = -1; q < meansize-1; q++)
                        {
                            if ((j + p) >= 0 && (j + p) < LineCounts && (k + q) >= 0 && (k + q) < ColumnCounts)
                            {
                                if (orglpixel[j + p, k + q] > max)
                                    max = orglpixel[j + p, k + q];
                                if (orglpixel[j + p, k + q] < min)
                                    min = orglpixel[j + p, k + q];
                            }
                        }
                    }
                    this.meanrange[j*ColumnCounts+k] = max - min;
                }
            }

            read rd = new read();
            rd.bands = 1;
            rd.ColumnCounts = this.ColumnCounts;
            rd.LineCounts = this.LineCounts;
            rd.DataType = 4;
            rd.BandsDataD = new double[rd.bands, ColumnCounts* LineCounts];
            rd.Bandsname = new string[1];
            rd.BandsData = new int[rd.bands, ColumnCounts * LineCounts];
            for (int j = 0; j < ColumnCounts * LineCounts; j++)
            {
                rd.BandsDataD[0, j] = this.meanrange[j];
                rd.BandsData[0, j] = (int)rd.BandsDataD[0, j];
            }
            rd.Bandsname[0] = "band" + (i + 1);
            rd.FileName = "rangeTexture";
            Form1.boduan.Add(rd);
            Form1.abl.readmore.Add(rd);
            Form1.abl.PATH = rd.FileName;
            Form1.abl.Form_Load(sender, e);
            Form1.abl.Show();
            this.Close();



        }
       /// <summary>
       /// 添加波段号
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void RangeTexture_Load(object sender, EventArgs e)
        {
            for (int h = 1; h <= bands; h++)
            {

                comboBox1.Items.Add("band" + h);
                
            }
        }
    }
}
