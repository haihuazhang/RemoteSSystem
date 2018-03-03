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
    public partial class Combine : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Combine()
        {
            InitializeComponent();
        }
        byte[] bits = new byte[30000000];
        //int ColumnCounts, LineCounts, DataType, bands;
        //string Interleaves;
        string PATH = @"..\\AA.hdr";//需要读取的头文件路径
        string[] bsqPATH = new string[7];//7个波段BSQ文件路径暂存的字符串数组
        int ColumnCounts, LineCounts, bands, DataType;//暂存的波段、行、列变量
        //string bilPATH = "";//写入BIL数据文件路径
        string createNewBSQ = "";//写入BSQ数据文件路径
        //string bipPATH = "";   //写入BIP数据文件路径
        //string BSQaPATH = "";
        //string BSQahdrPAHT = "";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = null;

            if (SFD.ShowDialog() == DialogResult.OK)
            {
                createNewBSQ = SFD.FileName;
                this.textEdit4.Text = SFD.FileName;

            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            bsqPATH[0] = @"..\\b1";
            bsqPATH[1] = @"..\\b2";
            bsqPATH[2] = @"..\\b3";
            bsqPATH[3] = @"..\\b4";
            bsqPATH[4] = @"..\\b5";
            bsqPATH[5] = @"..\\b6";
            bsqPATH[6] = @"..\\b7";
            BSQ bsq = new BSQ();//新建实例

            bsq.HDRread(PATH); //读取BSQ头文件，得到行、列、波段数；

            //.......................................................
            this.textEdit1.Text = bsq.LineCounts.ToString(); 
            this.textEdit2.Text = bsq.ColumnCounts.ToString(); 
            this.textEdit3.Text = bsq.bands.ToString();
            //窗体中显示行、列、波段数
            //.....................................
            this.ColumnCounts = bsq.ColumnCounts;
            this.LineCounts = bsq.LineCounts;
            this.bands = bsq.bands;
            this.DataType = bsq.DataType;

            bsq.BSQread(bsqPATH);
            for (int i = 0; i < bsq.totalnum; i++)
            {
                bits[i] = bsq.bits[i];
            }
            //Form1类与BSQ类的中的变量对应转换赋值
            //...........................................
            if (createNewBSQ == "")
            {
                MessageBox.Show("请输入路径！");
            }
            else
            {
                FileStream fs1 = new FileStream(createNewBSQ, FileMode.Create);
                for (int i = 0; i < bsq.LineCounts * bsq.bands; i++)
                {
                    for (int j = 0; j < bsq.ColumnCounts; j++)
                    {
                        fs1.WriteByte(bits[i * bsq.ColumnCounts + j]);
                    }
                }
                //写入BSQ数据文件，并命名
                //.................................
                WriteHdr whdr = new WriteHdr();
                string bsqhdrpath = createNewBSQ + ".hdr";
                whdr.BSQHDR(bsqhdrpath);
                //写入BSQ头文件，并命名
                MessageBox.Show("合并成功");
                fs1.Flush();
                fs1.Close();
                fs1.Dispose();
            }
        }
    }
}