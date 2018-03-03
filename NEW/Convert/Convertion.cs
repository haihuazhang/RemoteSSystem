using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace RemoteSystem
{
    public partial class Convertion : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Convertion()
        {
            InitializeComponent();
        }
        public byte[] bits;
        public int ColumnCounts, LineCounts, bands, DataType;
        string outputPATH = "";//写入输出数据文件路径
        string readHdrPath = "";
        //string BSQaPATH = "";
        //string BSQahdrPAHT = "";
        read rd;
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 读取待转文件信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = null;

            if (OFD.ShowDialog() == DialogResult.OK)
            {
                string Path = OFD.FileName;
                readHdrPath = Path+".hdr";
                rd = new read();
                rd.HDRread(readHdrPath);
                rd.Dataread(Path);
                textEdit2.Text = Path;
            }
        }


        /// <summary>
        /// 确定路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = null;

            if (SFD.ShowDialog() == DialogResult.OK)
            {
                outputPATH = SFD.FileName;
                textEdit1.Text = outputPATH;
            }
        }
        /// <summary>
        /// 开始数据转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string InterLeave = "";
            if (textEdit1.Text != "")
            {
                if (this.radioButton1.Checked)
                {
                    InterLeave = "bil";


                }
                else if (this.radioButton2.Checked)
                {
                    InterLeave = "bip";
                }
                else if (this.radioButton3.Checked)
                {
                    InterLeave = "bsq";
                }
                Write wit = new Write(rd, InterLeave, readHdrPath);
                wit.WriteData(outputPATH);
                wit.WriteHDR(outputPATH);
                MessageBox.Show("转换成功！");
                this.Close();
            }
            else
                MessageBox.Show("请输入正确路径！");
        }

        private void Convert_Load(object sender, EventArgs e)
        {

        }
    }
}