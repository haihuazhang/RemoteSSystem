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
    /// 波段运算窗口
    /// </summary>
    public partial class BandMathinput : Form
    {
        public BandMathinput()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 添加数据列表（树状结构）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BandMathinput_Load(object sender, EventArgs e)
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
                    textBox1.Text +='"'+ treeView1.SelectedNode.Text + ":" + treeView1.SelectedNode.Parent.Text+'"';
            }
        }
        /// <summary>
        /// 进行波段运算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                read rd = new read();
                Calc cc = new Calc(textBox1.Text);
                rd = cc.GetResult();
                if (!cc.isExpressionRight)
                {
                    MessageBox.Show("表达式不合法！");
                    return;
                }
                if (cc.ishandled)
                {
                    if (textBox3.Text == "")
                    {
                        rd.FileName = "BandMath";
                    }
                    else
                    {
                        rd.FileName = textBox3.Text;
                    }

                    rd.Bandsname[0] = textBox1.Text.Replace('"', ' ').Trim();



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
                {
                    MessageBox.Show("波段规格不匹配！");
                }
            }
            else
            {
                MessageBox.Show("请输入表达式！");
            }
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text += "+";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text += "-";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text += "*";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text += "/";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text += "(";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text += ")";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text += 1;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text += 2;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text += 3;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text += 4;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox1.Text += 5;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text += 6;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Text += 7;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox1.Text += 8;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox1.Text += 9;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            textBox1.Text += 10;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            textBox1.Text += '.';
        }

        private void button20_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
        }
    }
}
