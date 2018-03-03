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
    public partial class singleband : Form
    {
        public int[,] BandsData;
        public int[,] showdata;
        public double[,] BandsDataD;
        public int ColumnCounts, LineCounts;
        public int bands;
        public int[] pixel, overlay;
        public singleband(int ColumnCounts,int LineCounts,int bands)
        {
            InitializeComponent();
            BandsData = new int[bands,ColumnCounts*LineCounts];
            showdata = new int[bands, ColumnCounts * LineCounts];
            BandsDataD = new double[bands, ColumnCounts * LineCounts];
        }
        int[,] doublepixel;
        /// <summary>
        /// 画直方图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                //pixel = new int[256];
                Histogram f2 = new Histogram(null);
                f2.Height = 500;
                f2.Width = 500;
                int i = Convert.ToInt32(comboBox1.Text.Substring(4)) - 1; ;
                perHistogram phs = new perHistogram();


                phs.histogram2(BandsDataD, ColumnCounts * LineCounts, i, out pixel);
                f2.max = phs.max;
                f2.min = phs.min;
                f2.stretch = phs.stretch;
                f2.draw = new int[256];
                f2.draw = pixel;
                f2.Show();
            }
            else
                MessageBox.Show("请选择波段！");
        }

        /// <summary>
        /// 画累计直方图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                //pixel = new int[256];
                //overlay = new int[256];
                Histogram f2 = new Histogram(null);
                f2.Height = 500;
                f2.Width = 500;

                int i = Convert.ToInt32(comboBox1.Text.Substring(4)) - 1; ;
                perHistogram phs = new perHistogram();


                phs.histogram2(BandsDataD, this.ColumnCounts * this.LineCounts, i,out pixel);
                phs.totalhistogram2(pixel,out overlay);
                f2.max = phs.max;
                f2.min = phs.min;
                f2.stretch = phs.stretch;
                f2.draw = new int[256];
                f2.draw = overlay;
                f2.Show();
            }
            else
                MessageBox.Show("请选择波段！");
        }
        /// <summary>
        /// 共生矩阵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {


            if (textBox2.Text != "" && textBox3.Text != "" && comboBox1.Text != "")
            {
                int i = Convert.ToInt32(comboBox1.Text.Substring(4)) - 1; ;
                int a = Convert.ToInt32(this.textBox2.Text);
                int b = Convert.ToInt32(this.textBox3.Text);

                doublepixel = new int[256, 256];




                if (a >= 0 && b >= 0)
                {
                    for (int p = 0; p < this.LineCounts - b; p++)
                    {
                        for (int k = 0; k < this.ColumnCounts - a; k++)
                        {

                            doublepixel[showdata[i, k + p * ColumnCounts], showdata[i, k + p * ColumnCounts + a + b * ColumnCounts]]++;
                        }
                    }
                }
                else if (a < 0 && b < 0)
                {
                    for (int p = 0 - b; p < this.LineCounts; p++)
                    {
                        for (int k = 0 - a; k < this.ColumnCounts; k++)
                        {

                            doublepixel[showdata[i, k + p * ColumnCounts],
                            showdata[i, k + p * ColumnCounts + a + b * ColumnCounts]]++;
                        }
                    }
                }
                else if (a < 0 && b >= 0)
                {
                    for (int p = 0; p < this.LineCounts - b; p++)
                    {
                        for (int k = 0 - a; k < this.ColumnCounts; k++)
                        {

                            doublepixel[showdata[i, k + p * ColumnCounts], showdata[i, k + p * ColumnCounts + a + b * ColumnCounts]]++;
                        }
                    }
                }
                else if (a >= 0 && b < 0)
                {
                    for (int p = 0 - b; p < this.LineCounts; p++)
                    {
                        for (int k = 0; k < this.ColumnCounts - a; k++)
                        {

                            doublepixel[showdata[i, k + p * ColumnCounts], showdata[i, k + p * ColumnCounts + a + b * ColumnCounts]]++;
                        }
                    }
                }
                doublematrix dmx = new doublematrix();
                dmx.doublepixel = new int[256, 256];
                dmx.doublepixel = this.doublepixel;
                dmx.Show();
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("请选择波段！");
            }
            else
                MessageBox.Show("请输入合适的偏移量！");

          

        }
        /// <summary>
        /// 波段选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void singleband_Load(object sender, EventArgs e)
        {
            for (int h = 1; h <= bands; h++)
            {
                comboBox1.Items.Add("band" + h);     
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void singleband_Load(object sender, EventArgs e)
        //{
            
        //}
    }
}
