using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace RemoteSystem
{
    /// <summary>
    /// 选择HSI变换源数据
    /// </summary>
    public partial class RGBToHSIView : Form
    {
        public RGBToHSIView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 添加选择数据列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HSIDataChoseView_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Form1.boduan.Count; i++)
            {
                /// <summary>
                /// 添加文件名
                /// <summary>
                TreeNode root = new TreeNode();
                root.Text = Form1.boduan[i].FileName;
                treeView1.Nodes.Add(root);
                /// <summary>
                /// 添加波段名
                /// <summary>
                for (int j = 0; j < Form1.boduan[i].bands; j++)
                {
                    TreeNode leave = new TreeNode();
                    leave.Text = Form1.boduan[i].Bandsname[j];
                    root.Nodes.Add(leave);
                }
            }
            radioButton1.Checked = true;

        }
        /// <summary>
        /// 选择数据和波段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Parent != null)
            {
                if (radioButton1.Checked)
                {
                    textBox1.Text = treeView1.SelectedNode.Text + ":" + treeView1.SelectedNode.Parent.Text;
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
                else if (radioButton2.Checked)
                {
                    textBox2.Text = treeView1.SelectedNode.Text + ":" + treeView1.SelectedNode.Parent.Text;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;
                }
                else
                {
                    textBox3.Text = treeView1.SelectedNode.Text + ":" + treeView1.SelectedNode.Parent.Text;
                    radioButton3.Checked = false;
                    radioButton1.Checked = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// HSI转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                /// <summary>
                ///从list数据流中按文件名索引数据。
                /// <summary>
                string pathmark1 = Path.GetFileName(textBox1.Text);
                string pathmark2 = Path.GetFileName(textBox2.Text);
                string pathmark3 = Path.GetFileName(textBox3.Text);
                GetDataByFilename gdb = new GetDataByFilename();
                int iR = gdb.getnumber(Form1.boduan, pathmark1);
                int iG = gdb.getnumber(Form1.boduan, pathmark2);
                int iB = gdb.getnumber(Form1.boduan, pathmark3);
                /// <summary>
                /// 按波段名索引波段号
                /// <summary>
                GetBandByname gbn = new GetBandByname();
                int Rband = gbn.getnumber(Form1.boduan[iR].Bandsname, this.textBox1.Text.Substring(0, this.textBox1.Text.IndexOf(pathmark1) - 1)
                    , Form1.boduan[iR].bands);
                int Gband = gbn.getnumber(Form1.boduan[iG].Bandsname, this.textBox2.Text.Substring(0, this.textBox2.Text.IndexOf(pathmark2) - 1)
                    , Form1.boduan[iG].bands);
                int Bband = gbn.getnumber(Form1.boduan[iB].Bandsname, this.textBox3.Text.Substring(0, this.textBox3.Text.IndexOf(pathmark3) - 1)
                    , Form1.boduan[iB].bands);
                if (Form1.boduan[iR].ColumnCounts == Form1.boduan[iG].ColumnCounts &&
                    Form1.boduan[iR].ColumnCounts == Form1.boduan[iB].ColumnCounts
                    && Form1.boduan[iR].LineCounts == Form1.boduan[iG].LineCounts && Form1.boduan[iR].LineCounts == Form1.boduan[iB].LineCounts)
                {
                    read rd = new read();


                    /// <summary>
                    /// RGBtoHSI
                    /// <summary>
                    RGBToHSI rth = new RGBToHSI(iR, iG, iB, Rband, Gband, Bband, rd);
                    rd = rth.GetResult();
                    rd.Bandsname = new string[3];
                    rd.Bandsname[0] = "Hue" + "(" + Form1.boduan[iR].Bandsname[Rband] + ":" + pathmark1 + ")";
                    rd.Bandsname[1] = "Sat" + "(" + Form1.boduan[iG].Bandsname[Gband] + ":" + pathmark2 + ")";
                    rd.Bandsname[2] = "Iit" + "(" + Form1.boduan[iB].Bandsname[Bband] + ":" + pathmark3 + ")";

                    if (textBox4.Text == "")
                        rd.FileName = "HSI";
                    else
                        rd.FileName = textBox4.Text;
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
                else
                    MessageBox.Show("波段规格不匹配！");
            }
            else
                MessageBox.Show("无有效数据！");
        }
    }
}
