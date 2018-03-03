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
    public partial class Histogram : Form
    {

        public Histogram(PictureBox m_pictureBox)
        {
            InitializeComponent();
            this.m_pictureBox1 = m_pictureBox;
        }
        //public Histogram(PictureBox m_pictureBox)
        //{
        //    this.m_pictureBox1 = m_pictureBox;
        //}
        public PictureBox m_pictureBox1;
        /// <summary>
        /// 像素统计直方图直方图
        /// </summary>
        public int[] draw;
        /// <summary>
        /// x坐标拉伸使用
        /// </summary>
        public double max;
        public double min;
        public double stretch;
        /// <summary>
        /// 画图控件
        /// </summary>
        //public PictureBox pictureBox1 = new PictureBox();
        private Graphics g2;
        /// <summary>
        /// 是否进行图像分割
        /// </summary>
        public int flag = 0;
        /// <summary>
        /// 显示窗口名
        /// </summary>
        public string Windowname;
        /// <summary>
        /// 显窗口序号
        /// </summary>
        int WinN;
        /// <summary>
        /// 标记鼠标是否为按住状态
        /// <summary>
        private int MoveFlag = 0;
        public Bitmap map;
        /// <summary>
        /// 屏幕坐标和画布坐标（当前鼠标位置）
        /// </summary>
        Point pt2 = new Point(); Point pt1 = new Point();
        ImageShow ims = new ImageShow();
        ThresholdSplit ths = new ThresholdSplit();
        /// <summary>
        /// Form_Load事件中实现画图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Width = 360;
            pictureBox2.Dock = DockStyle.Fill;
            pictureBox2.BackColor = Color.White;
            pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            this.Controls.Add(pictureBox2);
            if (flag == 1)
            {
                GetWinByName gwbn = new GetWinByName();
                WinN = gwbn.GetWinN(Form1.abl.Wins, Windowname);
                map = new Bitmap(Form1.abl.Wins[WinN].pictureBox1.Width, Form1.abl.Wins[WinN].pictureBox1.Height);
            }
        }
        /// <summary>
        /// 画图事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)//引用自MSDN引例
        {
           

            
            // Draw a line in the PictureBox.

            /// <summary>
            /// 缩小倍数
            /// <summary>
            int Multiple = 0;
            /// <summary>
            /// 直方图统计量最大值
            /// <summary>
            int numMax=0;
            for (int j = 0; j < 256;j++ )
                if(draw[j]>numMax)
                    numMax=draw[j];
            Multiple = (int)(numMax / pictureBox2.Height*1.2);
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;
            /// <summary>
            ///坐标反转
            /// <summary>
            e.Graphics.TranslateTransform(30, e.ClipRectangle.Height * 14 / 15);
            e.Graphics.ScaleTransform(1, -1);
            /// <summary>
            /// 画X,Y轴
            /// <summary>
            g.DrawLine(System.Drawing.Pens.Gray, 0, 0, 0,(int)( pictureBox2.Height/1.2));
            g.DrawLine(System.Drawing.Pens.Gray, 0, 0, 255, 0);

            /// <summary>
            /// 画线
            /// <summary>
            for (int j = 0; j < 255; j++)
            {
                if (draw[j] == 0)
                { }
                else
                {
                    //int q = 0;
                    for (int p = 1; p <= 255 - j; p++)
                        if (draw[j+p] != 0)
                        {
                            g.DrawLine(System.Drawing.Pens.Red, j, draw[j] / Multiple, j + p, draw[j + p] / Multiple);
                            break;
                        }
                   
                }
            }
    
            /// <summary>
            ///恢复原有坐标系
            /// <summary>
            e.Graphics.ScaleTransform(1, -1);
            //e.Graphics.TranslateTransform(0, e.ClipRectangle.Height/2);
            /// <summary>
            /// Draw a string on the PictureBox.(x轴标记)
            /// <summary>
            for (int i = 0; i < 6; i++)
            {
                g.DrawString(Math.Round((min + stretch / 5 * i), 2).ToString(), new Font("Arial", 10), System.Drawing.Brushes.Black, (float)Math.Ceiling((double)255 * i / 5), 0);
                //画刻度
                g.DrawLine(System.Drawing.Pens.Gray, (float)Math.Ceiling((double)255 * i / 5), 0, (float)Math.Ceiling((double)255 * i / 5), -5);
            }

             /// <summary>
             /// y轴标记
             /// <summary>
             for (int i = 1; i < 4; i++)
             {
                 double y = Convert.ToDouble(numMax) / 4 / Multiple * i;
                 g.DrawString((numMax/4*i).ToString(), new Font("Arial", 10), System.Drawing.Brushes.Black, -30, -1*(int)y);
                 //画刻度
                 g.DrawLine(System.Drawing.Pens.Gray, 0, -1 * (int)y, 5, -1 * (int)y);
             }
             e.Dispose();
             
            
        }



        /// <summary>
        /// 按住鼠标，标记拖住状态为真，开始滑动阈值线，进行交互阈值分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {


            if (flag == 1)
            {
               
                g2 = pictureBox2.CreateGraphics();
                pictureBox2.Refresh();
                //g2.Dispose();
                g2.TranslateTransform(30, pictureBox2.Height * 14 / 15);
                pt1 = MousePosition;
                pt2 = pictureBox2.PointToClient(pt1);
                g2.DrawLine(System.Drawing.Pens.Gray, pt2.X - 30, 0, pt2.X - 30, -1 * (int)(pictureBox2.Height / 1.2));
                //标记鼠标为拖住状态
                MoveFlag = 1;

                Form1.abl.Wins[WinN].pictureBox1.Update();
                Form1.abl.Wins[WinN].bandstemp=ths.choseThre(Form1.abl.Wins[WinN].BandsDataD, 0, 
                    Form1.abl.Wins[WinN].ColumnCounts,Form1.abl.Wins[WinN].LineCounts, (pt2.X - 30)*stretch/255+min);
                ims.showimage(Form1.abl.Wins[WinN].bandstemp, Form1.abl.Wins[WinN].ColumnCounts,
                    Form1.abl.Wins[WinN].LineCounts
                    , 1, map);
                m_pictureBox1.Image = map;
            }
        }
        /// <summary>
        /// 移动阈值线，进行分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_MouseMove_1(object sender, MouseEventArgs e)
        {
           
            if (flag == 1)
            {
                if (MoveFlag == 1)
                {
                    g2 = pictureBox2.CreateGraphics();
                    pictureBox2.Refresh();
                    //g2.Dispose();
                    g2.TranslateTransform(30, pictureBox2.Height * 14 / 15);
                    pt1 = MousePosition;
                    pt2 = pictureBox2.PointToClient(pt1);
                    g2.DrawLine(System.Drawing.Pens.Gray, pt2.X - 30, 0, pt2.X - 30, -1 * (int)(pictureBox2.Height / 1.2));

                    Form1.abl.Wins[WinN].pictureBox1.Refresh();


                    ims.showimage(ths.choseThre(Form1.abl.Wins[WinN].BandsDataD, 0, Form1.abl.Wins[WinN].ColumnCounts,
                    Form1.abl.Wins[WinN].LineCounts, (pt2.X - 30) * stretch / 255 + min), 
                        Form1.abl.Wins[WinN].ColumnCounts, Form1.abl.Wins[WinN].LineCounts
                        , 1, map);
                    Form1.abl.Wins[WinN].pictureBox1.Image = map;

                }
                
            }
        }
        /// <summary>
        /// 结束阈值分割，并赋值标记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (flag == 1)
            {  
                Form1.abl.Wins[WinN].bandstemp=
                ths.choseThre(Form1.abl.Wins[WinN].BandsDataD, 0, Form1.abl.Wins[WinN].ColumnCounts,
                    Form1.abl.Wins[WinN].LineCounts, (pt2.X - 30) * stretch / 255 + min);
                MoveFlag = 0;
                g2.Dispose();
            }
        }

            


    }
}
