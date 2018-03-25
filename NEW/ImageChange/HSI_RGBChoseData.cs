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
    public partial class HSIToRGBView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public HSIToRGBView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 头文件信息显示盒
        /// </summary>
        private RichTextBox rB1 = new RichTextBox();
        /// <summary>
        /// 改变选择数据时头文件信息变换事件
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
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 执行HSI转RGB操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (listBoxControl1.Text != "")
            {
                /// <summary>
                /// 创建新数据流
                /// <summary>
                read rd = new read();
                /// <summary>
                /// 索引参与运算数据
                /// <summary>
                GetDataByFilename gdbf = new GetDataByFilename();
                int N = gdbf.getnumber(Form1.boduan, listBoxControl1.Text);
                HSIToRGB HtR = new HSIToRGB(N);
                if (!HtR.isHSI)
                {
                    MessageBox.Show("不是HSI数据！");
                    return;
                }
                rd = HtR.Getresult();
                if (textEdit1.Text == "")
                {
                    rd.FileName = "RGB";
                }
                else
                {
                    rd.FileName = textEdit1.Text;
                }

                rd.Bandsname[0] = "Rband";
                rd.Bandsname[1] = "Gband";
                rd.Bandsname[2] = "Bband";
                /// <summary>
                /// 结果图像压入静态数据流
                /// <summary>
                Form1.boduan.Add(rd);
                Form1.abl.readmore.Add(rd);
                /// <summary>
                /// availablebandlist中显示结果数据
                /// <summary>
                Form1.abl.PATH = rd.FileName;
                Form1.abl.Form_Load(sender, e);
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择数据！");
            }
        }
        /// <summary>
        /// 加载数据项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HSIToRGBView_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Form1.boduan.Count; i++)
            {
                listBoxControl1.Items.Add(Form1.boduan[i].FileName);
            }
        }
    }
}