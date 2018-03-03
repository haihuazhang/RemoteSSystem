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
    public partial class classInputFile : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public classInputFile()
        {
            InitializeComponent();
        }
        /// <summary>
        /// HRD信息消息盒
        /// </summary>
        private RichTextBox rB1 = new RichTextBox();
        /// <summary>
        /// 加载数据选择列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void classInputFile_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Form1.boduan.Count; i++)
                listBoxControl1.Items.Add(Form1.boduan[i].FileName);
        }
        /// <summary>
        /// 切换选择数据时改变头文件信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxControl1.Text == "")
            {
                MessageBox.Show("无数据！");
                return;
            }
            this.Controls.Remove(rB1);
            rB1 = new RichTextBox();
            showHdr sH = new showHdr();
            sH.richTextBox1.Width = 120;
            sH.richTextBox1.Location = new Point(150, 100);
            rB1 = sH.gethdr(listBoxControl1.SelectedItem.ToString());
            this.Controls.Add(rB1);
        }
        /// <summary>
        /// 进行分类操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (listBoxControl1.Text != "")
            {
                if (spinEdit1.Text == "")
                {
                    MessageBox.Show("请选择分类个数！");
                    return;
                }
                /// <summary>
                /// 创建新数据流
                /// <summary>
                read rd = new read();
                /// <summary>
                /// 索引参与运算数据
                /// <summary>
                GetDataByFilename gdbf = new GetDataByFilename();
                int N = gdbf.getnumber(Form1.boduan, listBoxControl1.Text);
                Kmean Km = new Kmean(N, Convert.ToInt16(spinEdit1.Text), 7);
                rd = Km.GetResult();
                if (textEdit1.Text == "")
                {
                    rd.FileName = "classFile";
                }
                else
                    rd.FileName = textEdit1.Text;
                rd.Bandsname = new string[1];
                rd.Bandsname[0] = "band1";


                Form1.boduan.Add(rd);
                Form1.abl.readmore.Add(rd);
                Form1.abl.PATH = rd.FileName;
                Form1.abl.Form_Load(sender, e);
                Form1.abl.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择数据！");
                this.Close();
            }
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}