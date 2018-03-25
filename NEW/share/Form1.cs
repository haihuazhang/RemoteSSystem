using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace RemoteSystem
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static AvailableBandsList abl = new AvailableBandsList();
        public int[,] BandsData;
        public int[,] showData;
        public string Interleave;

        public int ColumnCounts, LineCounts, bands;
        /// <summary>
        /// 为AvailableBandsList添加新树节点提供数据标识。
        /// </summary>
        public string DataPath;
        /// <summary>
        /// 当前数据流和静态数据流
        /// </summary>
        public List<read> readmore = new List<read>();
        public static List<read> boduan = new List<read>();

        private void OpenFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = null;

            if (OFD.ShowDialog() == DialogResult.OK)
            {
                /// <summary>
                /// 数据读取
                /// <summary>
                DataPath = OFD.FileName;
                string hdrPATH = DataPath + ".hdr";
                read rd = new read();
                rd.HDRread(hdrPATH);
                rd.Dataread(DataPath);
                /// <summary>
                /// 添加至数据流中
                readmore.Add(rd);
                boduan.Add(rd);
                this.BandsData = rd.BandsData;
                abl.readmore.Add(rd);

                /// <summary>
                /// 数据标识传递
                /// <summary>
                abl.PATH = DataPath;
                /// <summary>
                /// Form_Load方法添加新树节点
                abl.Form_Load(sender, e);
                /// <summary>
                /// show
                /// <summary>
                abl.Show();

            }
        }
        /// <summary>
        /// 波段统计功能类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /// <summary>
            /// chooseData窗体打开，并添加可选数据项（通过文件名索引）
            chooseData cd = new chooseData();
            for (int i = 0; i < Form1.boduan.Count; i++)
            {
                cd.listBox1.Items.Add(Form1.boduan[i].FileName);
            }

            cd.Show();
        }
        /// <summary>
        /// 文件格式转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Convertion cvt = new Convertion();

            cvt.Show();
        }
        /// <summary>
        /// 单波段文件合成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Combine w1 = new Combine();
            w1.Show();

        }
        /// <summary>
        /// K-T变换窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            K_T_Input_File ktinput = new K_T_Input_File();
            for (int i = 0; i < Form1.boduan.Count; i++)
            {
                ktinput.listBox1.Items.Add(Form1.boduan[i].FileName);
            }

            ktinput.Show();
        }
        /// <summary>
        /// RGB转HSI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RGBToHSIView hdcv = new RGBToHSIView();
            hdcv.Show();
        }
        /// <summary>
        /// 波段运算（代数运算）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void An_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BandMathinput bmi = new BandMathinput();
            bmi.Show();
        }
        /// <summary>
        /// 滤波窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            choseFileForFilter cfff = new choseFileForFilter();
            cfff.Show();
        }
        /// <summary>
        /// 显示AvailableBandsList窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            abl.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ribbonControl1.Minimized = true;

        }
        /// <summary>
        /// K均值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            classInputFile cIF = new classInputFile();
            cIF.Show();
        }
        /// <summary>
        /// HSI转RGB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HSIToRGBView HTRV = new HSIToRGBView();
            HTRV.Show();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                System.Diagnostics.Process.Start("help.chm");
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
