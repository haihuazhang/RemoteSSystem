using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.IO;

namespace RemoteSystem
{
    public partial class imageview : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        /// <summary>
        /// 构造窗口+命名
        /// </summary>
        /// <param name="windowname">构造窗口name</param>
        public imageview(string windowname)
        {
            InitializeComponent();
            this.windowname = windowname;
        }
        /// <summary>
        /// 0-255值域显示数据
        /// </summary>
        public int[,] showdata;
        /// <summary>
        /// 双精度原数据
        /// </summary>
        public double[,] BandsDataD;
        /// <summary>
        /// 波段数据
        /// </summary>
        public int ColumnCounts, LineCounts, bands;
        /// <summary>
        /// 窗口名称
        /// </summary>
        public string windowname;
        /// <summary>
        /// 图像增强缓存数据
        /// </summary>
        public int[,] bandstemp;
        /// <summary>
        /// 均衡化和规定化缓存数据
        /// </summary>
        private double[,] StretchTemp;
        /// <summary>
        /// R、G、B波段名
        /// </summary>
        private string[] Bandsname;
        /// <summary>
        /// 增强类型（均衡化或规定化）
        /// </summary>
        private string stretchtype;
        /// <summary>
        /// 是否为二值化数据
        /// </summary>
        public bool isValue;
        /// <summary>
        /// 窗口像素值查询窗口
        /// </summary>
        private showpixel sp;
        /// <summary>
        /// 像素值查询窗口是否打开
        /// </summary>
        static private bool isShowPixel;

        private Bitmap map;

        Point pt1;

        /// <summary>
        /// Form_load事件实现窗口部分数据初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageview_Load(object sender, EventArgs e)
        {
            ribbon.Minimized = true;
            //bandstemp用来计算显示图像数据
            bandstemp = new int[bands, ColumnCounts * LineCounts];
            BandpixelInitialize bpi = new BandpixelInitialize();
            bpi.Initialization(out bandstemp, showdata, ColumnCounts, LineCounts, bands);
            map = (Bitmap)pictureBox1.Image;
        }
        public void handleBopen()
        {
            isValue = false;
            isShowPixel = false;
            StretchTemp = new double[bands, ColumnCounts * LineCounts];
            Bandsname = new string[bands];
            stretchtype = "";
            for (int i = 0; i < bands; i++)
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                    StretchTemp[i, j] = BandsDataD[i, j];
        }
        /// <summary>
        /// 存储窗口数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            read rd = new read();

            if (!isValue)
            {
                if (stretchtype == "")
                    for (int i = 0; i < bands; i++)
                        Bandsname[i] = "band" + (i + 1);
                else
                    for (int i = 0; i < bands; i++)
                        Bandsname[i] = stretchtype + ":" + "band" + (i + 1);
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = null;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    rd.SaveData(ColumnCounts, LineCounts, bands, StretchTemp, Bandsname);

                    SaveFile sf = new SaveFile(rd, sfd.FileName, 4);
                    sf.SaveDataF(); sf.Savehdr();
                    MessageBox.Show("保存成功！");
                    rd.FileName = Path.GetFileNameWithoutExtension(sfd.FileName);
                    rd.bands = bands;
                    rd.ColumnCounts = ColumnCounts;
                    rd.LineCounts = LineCounts;

                    rd.Bandsname = Bandsname;
                    rd.BandsDataD = StretchTemp;
                    rd.BandsData = new int[bands, ColumnCounts * LineCounts];
                    for (int i = 0; i < bands; i++)
                        for (int j = 0; j < ColumnCounts * LineCounts; j++)
                            rd.BandsData[i, j] = (int)rd.BandsDataD[i, j];

                    Form1.boduan.Add(rd);
                    Form1.abl.readmore.Add(rd);
                    Form1.abl.PATH = rd.FileName;
                    Form1.abl.Form_Load(sender, e);
                    Form1.abl.Show();
                }


            }
            else if (isValue)
            {
                for (int i = 0; i < bands; i++)
                    for (int j = 0; j < ColumnCounts * LineCounts; j++)
                        StretchTemp[i, j] = (double)bandstemp[i, j]/255;
                for (int i = 0; i < bands; i++)
                    Bandsname[i] = "Binary" + ":" + "band" + (i + 1);
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = null;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    rd.SaveData(ColumnCounts, LineCounts, bands, StretchTemp, Bandsname);
                    SaveFile sf = new SaveFile(rd, sfd.FileName, 1);
                    sf.SaveDataF(); sf.Savehdr();
                    MessageBox.Show("保存成功！");
                    rd.FileName = Path.GetFileNameWithoutExtension(sfd.FileName);
                    rd.bands = bands;
                    rd.ColumnCounts = ColumnCounts;
                    rd.LineCounts = LineCounts;

                    rd.Bandsname = Bandsname;
                    rd.BandsDataD = StretchTemp;
                    rd.BandsData = new int[bands, ColumnCounts * LineCounts];
                    for (int i = 0; i < bands; i++)
                        for (int j = 0; j < ColumnCounts * LineCounts; j++)
                            rd.BandsData[i, j] = (int)rd.BandsDataD[i, j];
                    Form1.boduan.Add(rd);
                    Form1.abl.readmore.Add(rd);
                    Form1.abl.PATH = rd.FileName;
                    Form1.abl.Form_Load(sender, e);
                    Form1.abl.Show();
                }
            }
            
        }
        /// <summary>
        /// 2%拉伸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            BandpixelInitialize bpi = new BandpixelInitialize();//画图数组初始化
            bpi.Initialization(out bandstemp, showdata, ColumnCounts, LineCounts, bands);
            imagestretch imgstrch = new imagestretch(ColumnCounts, LineCounts, bands);
            bandstemp = imgstrch.TwoPercentStretch(BandsDataD);

            pictureBox1.Refresh();
            map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            ImageShow ims = new ImageShow();
            ims.showimage(bandstemp, ColumnCounts, LineCounts, bands, map);
            pictureBox1.Image = map;
            isValue = false;
        }
        /// <summary>
        /// 均衡化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            imagestretch imst = new imagestretch(ColumnCounts, LineCounts, bands);
            imst.Equalization(BandsDataD);
            StretchTemp = imst.getresult();

            this.bandstemp = imst.TwoPercentStretch(StretchTemp);
            pictureBox1.Refresh();
            map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            ImageShow ims = new ImageShow();
            ims.showimage(bandstemp, ColumnCounts, LineCounts, bands, map);
            pictureBox1.Image = map;
            isValue = false;
            stretchtype = "Equalization";
        }
        /// <summary>
        /// 直方图匹配（窗口间）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Form1.abl.Wins.Count == 1)
                MessageBox.Show("窗口不足，不能实现直方图匹配！");
            else
            {
                int WinN1, WinN2;
                HistogramMatch hm = new HistogramMatch();
                WinN1 = Convert.ToInt32(this.windowname.Substring(windowname.Length - 1));
                for (int i = 0; i < Form1.abl.Wins.Count; i++)
                    hm.listBox1.Items.Add(Form1.abl.Wins[i].windowname);
                hm.listBox1.Items.Remove(this.windowname);
                hm.ShowDialog();

                if (hm.Winnumber2 != -9999)
                {
                    WinN2 = hm.Winnumber2;
                    /// <summary>
                    /// 直方图匹配采用0-255值域显示数据方便步骤
                    /// <summary>
                    BandpixelInitialize bpi = new BandpixelInitialize();
                    bpi.Initialization(out bandstemp, showdata, ColumnCounts, LineCounts, bands);
                    imagestretch imst = new imagestretch(ColumnCounts, LineCounts, bands);
                    imst.HistogramMatch(this.BandsDataD, Form1.abl.Wins[WinN2].BandsDataD, Form1.abl.Wins[WinN2].bands);
                    StretchTemp = imst.getresult();
                    imst.LinearShow(bandstemp, StretchTemp);
                    /// <summary>
                    /// 显示图像
                    /// <summary>
                    pictureBox1.Refresh();
                    map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    ImageShow ims = new ImageShow();
                    ims.showimage(bandstemp, ColumnCounts, LineCounts, bands, map);
                    pictureBox1.Image = map;
                }
            }
            isValue = false;
            stretchtype = "HistogramMatch";
        }
        /// <summary>
        /// 任意范围拉伸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[][] pixel = new int[bands][];
            double[] max = new double[bands];
            double[] min = new double[bands];
            double[] stretch = new double[bands];
            for (int i = 0; i < bands; i++)
            {
                perHistogram ph = new perHistogram();
                ph.histogram2(BandsDataD, ColumnCounts * LineCounts, i, out pixel[i]);
                max[i] = ph.max;
                min[i] = ph.min;
                stretch[i] = ph.stretch;
            }
            ChooseRange CR = new ChooseRange();
            CR.bands = bands; CR.pixel = pixel;
            CR.max = max;
            CR.min = min;
            CR.stretch = stretch;
            //if (bands == 1)
            //{
            //    CR.g1 = CR.pictureBox1.CreateGraphics();
            //    CR.g1.TranslateTransform(30, CR.pictureBox1.Height * 14 / 15);
            //    CR.DrawRGB(0);
            //    CR.g2 = CR.pictureBox1.CreateGraphics();
            //    CR.g2.TranslateTransform(30, CR.pictureBox1.Height * 14 / 15);
            //    CR.g2.DrawLine(System.Drawing.Pens.Gray, CR.pt2.X, 0, CR.pt2.X, -1 * (int)(CR.pictureBox1.Height / 1.2));
            //    CR.g2.DrawLine(System.Drawing.Pens.Gray, CR.pt3.X, 0, CR.pt3.X, -1 * (int)(CR.pictureBox1.Height / 1.2));

            //}
            CR.ShowDialog();
            if (CR.WinState == true)
            {
                double a = CR.a;
                double b = CR.b;
                CR.WinState = false;
                BandpixelInitialize bpi = new BandpixelInitialize();//画图数组初始化
                bpi.Initialization(out bandstemp, showdata, ColumnCounts, LineCounts, bands);

                imagestretch imgstrch = new imagestretch(ColumnCounts, LineCounts, bands);
                imgstrch.GlobalLinear(BandsDataD, bandstemp, a, b, CR.rabuttonState);
                pictureBox1.Refresh();
                map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                ImageShow ims = new ImageShow();
                ims.showimage(bandstemp, ColumnCounts, LineCounts, bands, map);
                pictureBox1.Image = map;
                //CR.ShowDialog();
            }
            isValue = false;
        }
        /// <summary>
        /// 显示数据直方图查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            newHis nh = new newHis();
            nh.ColumnCounts = this.ColumnCounts;
            nh.LineCounts = this.LineCounts;
            nh.bands = this.bands;
            nh.StretchTemp = new double[nh.bands, nh.ColumnCounts * nh.LineCounts];
            nh.StretchTemp = this.StretchTemp;
            nh.Show();
        }
        /// <summary>
        /// 交互阈值分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (bands == 1)
            {
                perHistogram ph = new perHistogram();
                int[] pixel = new int[256];
                ph.histogram2(this.BandsDataD, ColumnCounts * LineCounts, 0, out pixel);
                Histogram h2 = new Histogram(pictureBox1);
                h2.Windowname = this.windowname;
                h2.flag = 1;
                h2.max = ph.max; h2.min = ph.min; h2.stretch = ph.stretch;
                h2.draw = pixel;
                isValue = true;
                h2.Show();

                //保存数据
                //map = h2.map;
                //int R = map.GetPixel(0, 0).R;
            }
            else
                MessageBox.Show("请选择单波段图像窗口!");
        }
        /// <summary>
        /// 腐蚀&膨胀
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (isValue)
            {
                int[,] kernel = new int[3, 3];
                Expand_Corode ec = new Expand_Corode();
                ec.ShowDialog();
                kernel = ec.kernel;
                if (ec.handelState == -1)
                { }
                else
                {
                    for (int i = 0; i < ColumnCounts * LineCounts; i++)
                        bandstemp[0, i] /= 255;
                    if (ec.handelState == 1)
                    {
                        Expand ep = new Expand(kernel);
                        bandstemp = ep.expand(bandstemp, 1, ColumnCounts, LineCounts);
                    }
                    else if (ec.handelState == 0)
                    {
                        Corrode cr = new Corrode(kernel);
                        bandstemp = cr.corrode(bandstemp, 1, ColumnCounts, LineCounts);
                    }
                    for (int i = 0; i < ColumnCounts * LineCounts; i++)
                        bandstemp[0, i] *= 255;
                    pictureBox1.Refresh();
                    map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    ImageShow ims = new ImageShow();
                    ims.showimage(bandstemp, ColumnCounts, LineCounts, 1, map);
                    pictureBox1.Image = map;
                    isValue = true;
                }

            }
            else
                MessageBox.Show("只能对二值化图像进行操作！");
        }
        /// <summary>
        /// OnClosing 事件重写
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {

            Form1.abl.Wins.Remove(this);
            //Form1.abl.windowsnum--;
            Form1.abl.comboBox1.Items.Remove(this.windowname);
            if (Form1.abl.comboBox1.Items.Count == 1)
                //Form1.abl.comboBox1.Items.Add("No Display");
                Form1.abl.comboBox1.SelectedText = "No Display";
            else
                Form1.abl.comboBox1.SelectedItem = Form1.abl.Wins[0].windowname;
            Form1.abl.TopMost = true;
            base.OnClosing(e);
        }
        /// <summary>
        /// 自适应阈值分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (bands == 1)
            {
                ThresholdSplit ths = new ThresholdSplit();
                bandstemp = ths.selfadapt(this.BandsDataD, 0, ColumnCounts, LineCounts);
                pictureBox1.Refresh();
                map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                ImageShow ims = new ImageShow();
                ims.showimage(bandstemp, ColumnCounts, LineCounts, 1, map);
                pictureBox1.Image = map;

                //保存数据

                isValue = true;
            }
            else
                MessageBox.Show("请选择单波段图像窗口！");
        }

        private void pixelToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        /// <summary>
        /// 显示图像中像素点像素值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (isShowPixel)
            {
                sp.Hide();
                isShowPixel = false;
            }
            else
            {
                sp = new showpixel();
                sp.orginData = new string[3];
                //sp.showData = new string[3];
                pt1 = pictureBox1.PointToClient(MousePosition);
                int R = map.GetPixel(pt1.X, pt1.Y).R;
                int G = map.GetPixel(pt1.X, pt1.Y).G;
                int B = map.GetPixel(pt1.X, pt1.Y).B;

               
                sp.label1.Text += "显示数据：";
                sp.label1.Text += "R:" + R + "\t";
                sp.label1.Text += "G:" + G + "\t";
                sp.label1.Text += "B:" + B + "\r\n";

                for (int i = 0; i < 3; i++)
                {
                    int k = 0;
                    if (bands == 1)
                        k = 0;
                    else
                        k = i;
                    sp.orginData[i] = this.BandsDataD[k, pt1.X + pt1.Y * ColumnCounts].ToString();

                }
                sp.label1.Text += "原数据：";
                sp.label1.Text += "R:" + sp.orginData[0] + "\r\n";
                sp.label1.Text += "G:" + sp.orginData[1] + "\r\n";
                sp.label1.Text += "B:" + sp.orginData[2] + "\r\n";


                sp.Controls.Add(sp.label1);
                sp.Show();
                isShowPixel = true;
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        } 
    }
}