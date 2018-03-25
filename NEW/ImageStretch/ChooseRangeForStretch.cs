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
    public partial class ChooseRange : Form
    {
        public ChooseRange()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 公共变量，用于父窗口值传递
        /// </summary>
        public double a, b;
        /// <summary>
        /// 绘图变量
        /// </summary>
        public Graphics g1;
        public Graphics g2;
        /// <summary>
        /// 直方图统计数组
        /// </summary>
        public int[][] pixel;
        public double[] max,min,stretch;
        /// <summary>
        /// 波段数
        /// </summary>
        public int bands;
        /// <summary>
        /// R,G,B缩小分量
        /// </summary>
        private int[] Multiple;
        /// <summary>
        /// 当前鼠标坐标点
        /// </summary>
        private Point pt1 = new Point();
        /// <summary>
        /// 双阈值X,Y点
        /// </summary>
        public Point pt2 = new Point(30, 0);     
        public Point pt3 = new Point(180,0);
        /// <summary>
        /// R，G，B分量切换状态
        /// </summary>
        public int rabuttonState=0;
        /// <summary>
        /// 鼠标状态（0悬停），（1选中左阈值线），（2选中右阈值线）
        /// </summary>
        private int mouseState = 0;
        /// <summary>
        /// 窗口状态（true为转回父窗口，false为在子窗口操作）
        /// </summary>
        public bool WinState;
        /// <summary>
        /// 确定拉伸系数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            a = Convert.ToDouble(textBox3.Text);
            b = Convert.ToDouble(textBox4.Text);
            WinState = true;
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseRange_Load(object sender, EventArgs e)
        {
            pictureBox1.Width = 360;
            Multiple = new int[bands];
            if (bands == 1)
            {
                panel1.Hide();
                radioButton1.Text = "Grey";
                radioButton1.Checked = false;
            }
            else if (bands == 3)
            {
                panel1.Show();
                radioButton1.Checked = true;
              
            }
            textBox3.Text = ((pt2.X - 1) * stretch[rabuttonState] / 255 + min[rabuttonState]).ToString();
            textBox4.Text = (pt3.X * stretch[rabuttonState] / 255 + min[rabuttonState]).ToString();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                //radioButton1.Checked = true;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                pictureBox1.Refresh();
                rabuttonState = 0;
                DrawRGB(0);
                g2 = pictureBox1.CreateGraphics();
                g2.TranslateTransform(30, pictureBox1.Height * 14 / 15);
                g2.DrawLine(System.Drawing.Pens.Gray, pt2.X, 0, pt2.X, -1 * (int)(pictureBox1.Height / 1.2));
                g2.DrawLine(System.Drawing.Pens.Gray, pt3.X, 0, pt3.X, -1 * (int)(pictureBox1.Height / 1.2));
            }
        }
        

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                //radioButton2.Checked = true;
                //radioButton1.Checked = false;
                //radioButton3.Checked = false;
                radioButton1.Checked = false;
                pictureBox1.Refresh();
                rabuttonState = 1;
                DrawRGB(1);
                g2 = pictureBox1.CreateGraphics();
                g2.TranslateTransform(30, pictureBox1.Height * 14 / 15);
                g2.DrawLine(System.Drawing.Pens.Gray, pt2.X, 0, pt2.X, -1 * (int)(pictureBox1.Height / 1.2));
                g2.DrawLine(System.Drawing.Pens.Gray, pt3.X, 0, pt3.X, -1 * (int)(pictureBox1.Height / 1.2));
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                //radioButton3.Checked = true;
                //radioButton1.Checked = false;
                //radioButton2.Checked = false;
                radioButton1.Checked = false;
                pictureBox1.Refresh();
                rabuttonState = 2;
                DrawRGB(2);
                g2 = pictureBox1.CreateGraphics();
                g2.TranslateTransform(30, pictureBox1.Height * 14 / 15);
                g2.DrawLine(System.Drawing.Pens.Gray, pt2.X, 0, pt2.X, -1 * (int)(pictureBox1.Height / 1.2));
                g2.DrawLine(System.Drawing.Pens.Gray, pt3.X, 0, pt3.X, -1 * (int)(pictureBox1.Height / 1.2));
            }
        }

        public void DrawRGB(int key)
        {
            g1 = pictureBox1.CreateGraphics();
            g1.TranslateTransform(30, pictureBox1.Height * 14 / 15);
            int numMax = 0;
            for (int j = 0; j < 256; j++)
            {
                if (pixel[key][j] > numMax)
                {
                    numMax = pixel[key][j];
                }
            }

            Multiple[key] = (int)(numMax / this.pictureBox1.Height * 1.2);

            g1.DrawLine(System.Drawing.Pens.Gray, 0, 0, 0, -1 * (int)(this.pictureBox1.Height / 1.2));
            g1.DrawLine(System.Drawing.Pens.Gray, 0, 0, 255, 0);
            for (int j = 0; j < 255; j++)
            {
                if (pixel[key][j] == 0)
                { }
                else
                {
                    //int q = 0;
                    for (int p = 1; p <= 255 - j; p++)
                    {
                        if (pixel[key][j + p] != 0)
                        {
                            g1.DrawLine(System.Drawing.Pens.Red, j, -1 * pixel[key][j] / Multiple[key], j + p, -1 * pixel[key][j + p] / Multiple[key]);
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// Draw a string on the PictureBox.(画x轴标记)
            /// <summary>
            for (int i = 0; i < 6; i++)
            {
                g1.DrawString(Math.Round((min[key] + stretch[key] / 5 * i), 2).ToString(), new Font("Arial", 10), System.Drawing.Brushes.Black, (float)Math.Ceiling((double)255 * i / 5), 0);
            }

            /// y轴标记
            /// <summary>
            for (int i = 1; i <= 4; i++)
            {
                double y = pictureBox1.Height*i / 4 /1.2 ;
                g1.DrawString((numMax / 4 * i).ToString(), new Font("Arial", 10), System.Drawing.Brushes.Black, -30, -1 * (int)y);
            }
            g1.Dispose();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pt1 = MousePosition;
            pt1 = pictureBox1.PointToClient(pt1);
            if (Math.Abs(pt1.X-30 -pt2.X)<=10)
            {
                mouseState = 1;
            }
            else if (Math.Abs(pt1.X-30-pt3.X)<=10)
            {
                mouseState = 2;
            }
            else
            {
                mouseState = 0;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseState = 0;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            switch (mouseState)
            {
                case 0:
                    break;
                case 1:
                    pictureBox1.Refresh();
                    DrawRGB(rabuttonState);
                    g2 = pictureBox1.CreateGraphics();
                    g2.TranslateTransform(30, pictureBox1.Height * 14 / 15);
                    pt1 = MousePosition;
                    pt2.X = pictureBox1.PointToClient(pt1).X-30;
                    g2.DrawLine(System.Drawing.Pens.Gray, pt2.X , 0, pt2.X , -1 * (int)(pictureBox1.Height / 1.2));
                    g2.DrawLine(System.Drawing.Pens.Gray, pt3.X , 0, pt3.X , -1 * (int)(pictureBox1.Height / 1.2));
                    //a处阈值要减一后变换回原数据阈值
                    textBox3.Text = ((pt2.X-1) * stretch[rabuttonState] / 255 + min[rabuttonState]).ToString();
                    

                    break;
                case 2:
                    pictureBox1.Refresh();
                    DrawRGB(rabuttonState);
                    g2 = pictureBox1.CreateGraphics();
                    g2.TranslateTransform(30, pictureBox1.Height * 14 / 15);
                    pt1 = MousePosition;
                    pt3.X = pictureBox1.PointToClient(pt1).X-30;
                    
                    g2.DrawLine(System.Drawing.Pens.Gray, pt3.X, 0, pt3.X , -1 * (int)(pictureBox1.Height / 1.2));
                    g2.DrawLine(System.Drawing.Pens.Gray, pt2.X , 0, pt2.X , -1 * (int)(pictureBox1.Height / 1.2));
                    textBox4.Text = (pt3.X * stretch[rabuttonState] / 255 + min[rabuttonState]).ToString();
                    
                    break;

            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
