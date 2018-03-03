using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace RemoteSystem
{
    public partial class newHis : DevExpress.XtraEditors.XtraForm
    {
        public newHis()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 波段数
        /// </summary>
        public int bands;
        /// <summary>
        /// 行列数
        /// </summary>
        public int ColumnCounts, LineCounts;
        /// <summary>
        /// 显示数据
        /// </summary>
        public double[,] StretchTemp;
        public double[] max, min, stretch;
        private int[] Multiple;
        /// <summary>
        /// 像素直方图
        /// </summary>
        public int[][] pixel;
        /// <summary>
        /// 缩小倍数
        /// <summary>

        /// <summary>
        /// 三波段绘图函数
        /// </summary>
        Graphics g, g1;
        //private PictureBox pictureBox2;
        /// <summary>
        /// Form_Load函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newHis_Load(object sender, EventArgs e)
        {
            Multiple = new int[bands];
            panel1.Location = new Point(0, 0);
            panel1.Show();
            /// <summary>
            /// 添加波段选择项
            /// <summary>
            if (bands == 1)
            {
                checkedListBox1.Items.Add("band0");
            }
            else
            {
                checkedListBox1.Items.Add("Rband");
                checkedListBox1.Items.Add("Gband");
                checkedListBox1.Items.Add("Bband");
            }
            /// <summary>
            /// 计算波段像素直方图
            /// <summary>
            pixel = new int[bands][];
            max = new double[bands];
            min = new double[bands];
            stretch = new double[bands];
            for (int i = 0; i < bands; i++)
            {
                perHistogram ph = new perHistogram();
                ph.histogram2(StretchTemp, ColumnCounts * LineCounts, i, out pixel[i]);
                max[i] = ph.max; min[i] = ph.min; stretch[i] = ph.stretch;
            }
            pictureBox1.Height = 300;
            pictureBox1.Width = 300;
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
        }
        /// <summary>
        /// 画直方图
        /// </summary>
        /// <param name="key"></param>
        public void DrawRGB(int key)
        {
            g1 = pictureBox1.CreateGraphics();
            g1.TranslateTransform(30, pictureBox1.Height * 14 / 15);
            int numMax = 0;
            for (int j = 0; j < 256; j++)
                if (pixel[key][j] > numMax)
                    numMax = pixel[key][j];
            Multiple[key] = (int)(numMax / this.pictureBox1.Height * 1.2);
            Pen pen = new Pen(Color.Black, 0);
            if (key == 0)
                pen = new Pen(Color.Red, 0);
            else if (key == 1)
                pen = new Pen(Color.Green, 0);
            else if (key == 2)
                pen = new Pen(Color.Blue, 0);

            for (int j = 0; j < 255; j++)
            {
                if (pixel[key][j] == 0)
                { }
                else
                {
                    //int q = 0;
                    for (int p = 1; p <= 255 - j; p++)
                        if (pixel[key][j + p] != 0)
                        {
                            g1.DrawLine(pen, j, -1 * pixel[key][j] / Multiple[key], j + p, -1 * pixel[key][j + p] / Multiple[key]);
                            break;
                        }
                }
            }

            /// <summary>
            /// Draw a string on the PictureBox.(画x轴标记)
            /// <summary>
            for (int i = 0; i < 6; i++)
            {
                g1.DrawString(Math.Round((min[key] + stretch[key] / 5 * i), 2).ToString(),
                    new Font("Arial", 10), System.Drawing.Brushes.Black,
                    (float)Math.Ceiling((double)255 * i / 5), 0);
                //画刻度
                g1.DrawLine(System.Drawing.Pens.Gray, (float)Math.Ceiling((double)255 * i / 5), 0, (float)Math.Ceiling((double)255 * i / 5), -5);
            }


            /// y轴标记
            /// <summary>
            for (int i = 1; i <= 4; i++)
            {
                double y = pictureBox1.Height * i / 4 / 1.2;
                g1.DrawString((numMax / 4 * i).ToString(), new Font("Arial", 10), System.Drawing.Brushes.Black, -30, -1 * (int)y);
                //画刻度
                g1.DrawLine(System.Drawing.Pens.Gray, 0, -1 * (int)y, 5, -1 * (int)y);
            }
            g1.Dispose();
        }
        /// <summary>
        /// 画坐标轴事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)//引用自MSDN引例
        {
            g = e.Graphics;
            /// <summary>
            /// 画横坐标
            /// <summary>
            g.TranslateTransform(30, pictureBox1.Height * 14 / 15);
            /// <summary>
            /// 画X,Y轴
            /// <summary>
            g.DrawLine(System.Drawing.Pens.Gray, 0, 0, 0, -1 * (int)(pictureBox1.Height / 1.2));
            g.DrawLine(System.Drawing.Pens.Gray, 0, 0, 255, 0);
            //    g.DrawString("0", new Font("Arial", 10), System.Drawing.Brushes.Black, 0, 0);
            //    g.DrawString("50", new Font("Arial", 10), System.Drawing.Brushes.Black, 50, 0);
            //    g.DrawString("100", new Font("Arial", 10), System.Drawing.Brushes.Black, 100, 0);
            //    g.DrawString("150", new Font("Arial", 10), System.Drawing.Brushes.Black, 150, 0);
            //    g.DrawString("200", new Font("Arial", 10), System.Drawing.Brushes.Black, 200, 0);
            //    g.DrawString("255", new Font("Arial", 10), System.Drawing.Brushes.Black, 255, 0);

            //    g.DrawLine(System.Drawing.Pens.Gray, 0, 0, 0, -1*(int)(pictureBox1.Height / 1.2));
            //    g.DrawLine(System.Drawing.Pens.Gray, 0, 0, 255, 0);


            //    /// <summary>
            //    /// 直方图统计量最大值
            //    /// <summary>
            //    int numMax = 0;
            //    for (int i = 0; i < bands; i++)
            //        for (int j = 0; j < 256; j++)
            //            if (pixel[i][j] > numMax)
            //                numMax = pixel[i][j];
            //    /// <summary>
            //    /// 缩小尺度
            //    /// <summary>
            //    Multiple = (int)(numMax / pictureBox1.Height * 1.2);
            //    /// <summary>
            //    /// y轴标记
            //    /// <summary>
            //    for (int i = 1; i < 4; i++)
            //    {
            //        double y = Convert.ToDouble(numMax) / 4 / Multiple * i;
            //        g.DrawString((numMax / 4 * i).ToString(), new Font("Arial", 10), System.Drawing.Brushes.Black, -30, -1 * (int)y);
            //    }

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_CheckMemberChanged(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 切换R,G,B按钮直方图变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (bands == 3)
            {
                if (checkedListBox1.SelectedItem.ToString() == "Rband")
                {
                    /// <summary>
                    /// 勾选R波段后
                    /// <summary>
                    //g1 = pictureBox1.CreateGraphics();
                    //g1.TranslateTransform(30, pictureBox1.Height * 14 / 15);
                    if (!checkedListBox1.GetItemChecked(0))
                    {
                        checkedListBox1.SetItemChecked(0, true);
                        //MessageBox.Show("Rband");
                        checkedListBox1.SetSelected(0, false);
                        DrawRGB(0);
                        //for (int i = 0; i < 255; i++)
                        //    g1.DrawLine(System.Drawing.Pens.Red, i, -1 * pixel[0][i] / Multiple[i], i + 1, -1 * pixel[0][i + 1] / Multiple[i]);

                    }
                    /// <summary>
                    /// 取消勾选R波段后
                    /// <summary>
                    else
                    {
                        checkedListBox1.SetItemChecked(0, false);
                        //MessageBox.Show("nullR");
                        checkedListBox1.SetSelected(0, false);
                        pictureBox1.Refresh();


                    }
                }
                else if (checkedListBox1.SelectedItem.ToString() == "Gband")
                {
                    /// <summary>
                    /// 勾选G波段后
                    /// <summary>
                    //g2 = pictureBox1.CreateGraphics();
                    //g2.TranslateTransform(30, pictureBox1.Height * 14 / 15);
                    if (!checkedListBox1.GetItemChecked(1))
                    {
                        checkedListBox1.SetItemChecked(1, true);
                        //MessageBox.Show("Gband");
                        checkedListBox1.SetSelected(1, false);
                        DrawRGB(1);
                        //for (int i = 0; i < 255; i++)
                        //    g2.DrawLine(System.Drawing.Pens.Green, i, -1 * pixel[1][i] / Multiple[i], i + 1, -1 * pixel[1][i + 1] / Multiple[i]);


                    }
                    /// <summary>
                    /// 取消勾选G波段后
                    /// <summary>
                    else
                    {
                        checkedListBox1.SetItemChecked(1, false);
                        //MessageBox.Show("nullG");
                        checkedListBox1.SetSelected(1, false);
                        pictureBox1.Refresh();

                    }
                }
                else if (checkedListBox1.SelectedItem.ToString() == "Bband")
                {
                    /// <summary>
                    /// 勾选B波段后
                    /// <summary>
                    //g3 = pictureBox1.CreateGraphics();
                    //g3.TranslateTransform(30, pictureBox1.Height * 14 / 15);
                    if (!checkedListBox1.GetItemChecked(2))
                    {
                        checkedListBox1.SetItemChecked(2, true);
                        //MessageBox.Show("Bband");
                        checkedListBox1.SetSelected(2, false);
                        DrawRGB(2);
                        //for (int i = 0; i < 255; i++)
                        //    g3.DrawLine(System.Drawing.Pens.Blue, i, -1 * pixel[2][i] / Multiple[i], i + 1, -1 * pixel[2][i + 1] / Multiple[i]);

                    }
                    /// <summary>
                    /// 取消勾选B波段后
                    /// <summary>
                    else
                    {
                        checkedListBox1.SetItemChecked(2, false);
                        //MessageBox.Show("nullB");
                        checkedListBox1.SetSelected(2, false);
                        pictureBox1.Refresh();

                    }
                }
            }
            else if (bands == 1)
            {
                if (checkedListBox1.SelectedItem.ToString() == "band0")
                {
                    //g2 = pictureBox1.CreateGraphics();
                    //g2.TranslateTransform(30, pictureBox1.Height * 14 / 15);
                    if (!checkedListBox1.GetItemChecked(0))
                    {
                        checkedListBox1.SetItemChecked(0, true);
                        //MessageBox.Show("Gband");
                        checkedListBox1.SetSelected(0, false);
                        DrawRGB(0);
                        //for (int i = 0; i < 255; i++)
                        //    g2.DrawLine(System.Drawing.Pens.Green, i, -1 * pixel[0][i] / Multiple[i], i + 1, -1 * pixel[0][i + 1] / Multiple[i]);
                    }
                }
            }
        }
      
    }
}