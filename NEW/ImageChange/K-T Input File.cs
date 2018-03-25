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
    /// K-T变换选择原数据和计算窗口
    /// </summary>
    public partial class K_T_Input_File : Form
    {
        public K_T_Input_File()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 文件名为窗体间传递唯一标识，利用文件名实现数据的查找
        /// </summary>
        public string filename;

        private RichTextBox rB1 = new RichTextBox();
        /// <summary>
        /// 关闭该窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// OK按钮计算目标数据的K-T变换结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void button1_Click(object sender, EventArgs e)
        {
            
            filename = listBox1.Text;
            GetDataByFilename gdbf = new GetDataByFilename();
            int pos = gdbf.getnumber(Form1.boduan, filename);
            if (listBox1.Text == "")
            {
                MessageBox.Show("无有效数据！");
                this.Close();
            }
            else if (Form1.boduan[pos].bands < 6)
            {
                MessageBox.Show("文件波段不满6！");
            }
            else
            {
                /// <summary>
                ///K-T变换结果数据流初始化
                /// </summary>
                read rd = new read();
                //进行K_T变换
                /// <summary>
                ///K-T变换计算，数据传递
                /// <summary>

                K_Tchange ktc = new K_Tchange(pos);

                rd = ktc.GetResult();
                rd.Bandsname = new string[3];

                rd.Bandsname[0] = "Brightness" + "(" + filename + ")";
                rd.Bandsname[1] = "Greenness" + "(" + filename + ")";
                rd.Bandsname[2] = "Third" + "(" + filename + ")";
                if (textBox1.Text == "")
                {
                    rd.FileName = "KT";
                }
                else
                {
                    rd.FileName = textBox1.Text;
                }

                /// <summary>
                ///静态窗口available打开，将结果加入静态数据流泛型中
                /// <summary>
                Form1.abl.readmore.Add(rd);
                Form1.abl.PATH = rd.FileName;
                Form1.boduan.Add(rd);
                Form1.abl.Form_Load(sender, e);
                Form1.abl.Show();


                this.Close();
            }
        }

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
            sH.richTextBox1.Location = new Point(180, 22);
            rB1 = sH.gethdr(listBox1.SelectedItem.ToString());
            this.Controls.Add(rB1);
        }
    }
}
