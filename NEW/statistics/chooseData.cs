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
    /// 根据文件名列表，选择合适数据
    /// </summary>
    public partial class chooseData : Form
    {
        public chooseData()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 头文件信息显示控件
        /// </summary>
        private RichTextBox rB1 = new RichTextBox();
        /// <summary>
        /// 关闭该窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 传递数据至image stastics窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            /// </summary>
            /// 根据文件名选择数据
            /// </summary>
            if (listBox1.Text == "")
            {
                MessageBox.Show("请选择数据！");
                return;
            }
            Imagestatistics imagsta = new Imagestatistics();
            imagsta.FileName = listBox1.SelectedItem.ToString();
            GetDataByFilename gdb =new GetDataByFilename();
            int DataNumber=gdb.getnumber(Form1.boduan,imagsta.FileName);
            /// </summary>
            /// imagsta实例窗体数据初始化和赋值
            /// </summary>
            imagsta.LineCounts = Form1.boduan[DataNumber].LineCounts;
            imagsta.ColumnCounts = Form1.boduan[DataNumber].ColumnCounts;
            imagsta.bands = Form1.boduan[DataNumber].bands;
            //imagsta.DataType = Form1.boduan[DataNumber].DataType;
            imagsta.Interleave = Form1.boduan[DataNumber].Interleave;
            imagsta.BandsData = new int[imagsta.bands, imagsta.ColumnCounts * imagsta.LineCounts];
            imagsta.BandsDataD = new double[imagsta.bands, imagsta.ColumnCounts * imagsta.LineCounts];
            imagsta.showdata = new int[imagsta.bands, imagsta.ColumnCounts * imagsta.LineCounts];
            for (int i = 0; i < imagsta.bands; i++)
            {
                for (int j = 0; j < imagsta.ColumnCounts * imagsta.LineCounts; j++)
                {
                    imagsta.BandsData[i, j] = Form1.boduan[DataNumber].BandsData[i, j];
                    imagsta.BandsDataD[i, j] = Form1.boduan[DataNumber].BandsDataD[i, j];
                }
            }

            /// </summary>
            /// 生成0-255范围像素数据，用于联合直方图和共生矩阵
            /// </summary>
            imagestretch ims = new imagestretch(imagsta.ColumnCounts, imagsta.LineCounts, imagsta.bands);
            ims.LinearShow(imagsta.showdata, imagsta.BandsDataD);
            imagsta.Show();
            this.Close();
        }
        /// <summary>
        /// 打开新文件作为数据传入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            /// </summary>
            /// 新文件文件读取
            /// </summary>
            string dataPath="";string hdrPath="";
            Imagestatistics imagsta = new Imagestatistics();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = null;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                 dataPath = ofd.FileName;
                 hdrPath = ofd.FileName + ".hdr";

            }
            read rd = new read();
            rd.HDRread(hdrPath);
            /// </summary>
            /// imagsta实例窗体数据初始化和赋值
            /// </summary>
            imagsta.LineCounts = rd.LineCounts;
            imagsta.ColumnCounts = rd.ColumnCounts;
            imagsta.bands = rd.bands;
            //imagsta.DataType = rd.DataType;
            imagsta.Interleave = rd.Interleave;
            imagsta.BandsData = new int[imagsta.bands, imagsta.ColumnCounts * imagsta.LineCounts];
            imagsta.BandsDataD = new double[imagsta.bands, imagsta.ColumnCounts * imagsta.LineCounts];
            //Form1.abl.readmore.Add(rd);

            if (rd.Dataread(dataPath) == true)
            {
                MessageBox.Show("选择成功！");
            }
            imagsta.BandsData = rd.BandsData;
            imagsta.BandsDataD = rd.BandsDataD;
            imagsta.Show();

            this.Close();
        }
        /// <summary>
        /// 切换选择数据，显示头文件信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.Text == "")
            {
                MessageBox.Show("无数据！");
                return;
            }
            this.Controls.Remove(rB1);
            rB1 = new RichTextBox();
            showHdr sH = new showHdr();
            sH.richTextBox1.Width = 120;
            sH.richTextBox1.Location = new Point(180, 34);
            rB1 = sH.gethdr(listBox1.SelectedItem.ToString());
            this.Controls.Add(rB1);
        }
    }
}
