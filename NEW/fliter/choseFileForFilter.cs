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
    public partial class choseFileForFilter : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public choseFileForFilter()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 头文件信息显示盒
        /// </summary>
        private RichTextBox rB1=new RichTextBox();
        /// <summary>
        /// 加载数据选择表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void choseFileForFilter_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Form1.boduan.Count; i++)
            {
                listBoxControl1.Items.Add(Form1.boduan[i].FileName);
            }

            comboBox1.Items.Add("Smoothing");
            comboBox1.Items.Add("Sharpen");
            comboBox1.SelectedItem = "Smoothing";

        }
        /// <summary>
        /// 切换平滑||锐化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //listBoxControl2.Location = new Point(160, 25);
            if (comboBox1.SelectedItem.ToString() == "Smoothing")
            {
                listBoxControl2.Items.Clear();
                listBoxControl2.Items.Add("ChooseMask");
                listBoxControl2.Items.Add("Gradient Inverse Weight");
                listBoxControl2.Items.Add("Median");
                listBoxControl2.Items.Add("Average");
                listBoxControl2.Items.Add("Gass Low Pass");
            }
            else if (comboBox1.SelectedItem.ToString()== "Sharpen")
            {
                listBoxControl2.Items.Clear();
                listBoxControl2.Items.Add("Laplacian");
                listBoxControl2.Items.Add("Prewitt");
                listBoxControl2.Items.Add("Sobel");
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
        /// <summary>
        /// 进行滤波操作！
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (listBoxControl1.Text != "" && listBoxControl2.Text != "")
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

                if (comboBox1.Text == "Smoothing")
                {
                    /// <summary>
                    /// 梯度倒数加权平均
                    /// <summary>
                    if (listBoxControl2.Text == "Gradient Inverse Weight")
                    {
                        GradientInverseWeight giw = new GradientInverseWeight(Form1.boduan[N].ColumnCounts, Form1.boduan[N].LineCounts, Form1.boduan[N].bands);
                        giw.Gradient(Form1.boduan[N].BandsDataD);
                        rd = giw.GetResult();
                        if (textEdit1.Text == "")
                        {
                            rd.FileName = "GradientInverseWeight";
                        }
                        else
                        {
                            rd.FileName = textEdit1.Text;
                        }

                        for (int i = 0; i < rd.bands; i++)
                        {
                            rd.Bandsname[i] = "GIW: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                        }
                    }
                    /// <summary>
                    /// 选择式掩模平滑
                    /// <summary>
                    else if (listBoxControl2.Text == "ChooseMask")
                    {
                        ChooseMask cm = new ChooseMask(Form1.boduan[N].ColumnCounts, Form1.boduan[N].LineCounts, Form1.boduan[N].bands);
                        cm.ChoseMaskPerf(Form1.boduan[N].BandsDataD);
                        rd = cm.GetResult();
                        if (textEdit1.Text == "")
                        {
                            rd.FileName = "ChooseMask";
                        }
                        else
                        {
                            rd.FileName = textEdit1.Text;
                        }

                        for (int i = 0; i < rd.bands; i++)
                        {
                            rd.Bandsname[i] = "CM: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                        }
                    }
                    /// <summary>
                    /// 均值滤波
                    /// <summary>
                    else if (listBoxControl2.Text == "Average")
                    {
                        Average_Median am = new Average_Median(Form1.boduan[N].ColumnCounts, Form1.boduan[N].LineCounts, Form1.boduan[N].bands);
                        am.AverSmoothing(Form1.boduan[N].BandsDataD);
                        rd = am.GetResult();
                        if (textEdit1.Text == "")
                        {
                            rd.FileName = "AverageSmooth";
                        }
                        else
                        {
                            rd.FileName = textEdit1.Text;
                        }

                        for (int i = 0; i < rd.bands; i++)
                        {
                            rd.Bandsname[i] = "Aver: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                        }
                    }
                    /// <summary>
                    /// 中值滤波
                    /// <summary>
                    else if (listBoxControl2.Text == "Median")
                    {
                        Average_Median am = new Average_Median(Form1.boduan[N].ColumnCounts, Form1.boduan[N].LineCounts, Form1.boduan[N].bands);
                        am.MedianSmoothing(Form1.boduan[N].BandsDataD);
                        rd = am.GetResult();
                        if (textEdit1.Text == "")
                        {
                            rd.FileName = "MedianSmooth";
                        }
                        else
                        {
                            rd.FileName = textEdit1.Text;
                        }

                        for (int i = 0; i < rd.bands; i++)
                        {
                            rd.Bandsname[i] = "Median: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                        }
                    }
                    else if (listBoxControl2.Text == "Gass Low Pass")
                    {

                        GassLowPass glp = new GassLowPass(Form1.boduan[N].ColumnCounts, Form1.boduan[N].LineCounts, Form1.boduan[N].bands);
                        glp.GetGassValue(Form1.boduan[N].BandsDataD, 5, 1);
                        rd = glp.GetResult();
                        if (textEdit1.Text == "")
                        {
                            rd.FileName = "Gass Low Pass";
                        }
                        else
                        {
                            rd.FileName = textEdit1.Text;
                        }

                        for (int i = 0; i < rd.bands; i++)
                        {
                            rd.Bandsname[i] = "GLP: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                        }
                    }
                }
                else if (comboBox1.Text == "Sharpen")
                {
                    /// <summary>
                    /// 梯度数据
                    /// <summary>
                    read rd2 = new read();
                    if (listBoxControl2.Text == "Laplacian")
                    {
                        Laplacian lpc = new Laplacian(Form1.boduan[N].ColumnCounts, Form1.boduan[N].LineCounts, Form1.boduan[N].bands);
                        lpc.Lapl(Form1.boduan[N].BandsDataD);
                        /// <summary>
                        /// 结果赋值
                        /// <summary>
                        rd = lpc.GetResult();
                        rd2 = lpc.GetGrad();



                        /// <summary>
                        /// 文件名
                        /// <summary>
                        if (textEdit1.Text == "")
                        {
                            rd.FileName = "LaplacianR";
                            rd2.FileName = "LaplacianG";
                        }
                        else
                        {
                            rd.FileName = textEdit1.Text;
                            rd2.FileName = textEdit1.Text + "_G";
                        }
                        /// <summary>
                        /// 波段名
                        /// <summary>
                        for (int i = 0; i < rd.bands; i++)
                        {
                            rd.Bandsname[i] = "LpR: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                            rd2.Bandsname[i] = "LpG: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                        }
                        /// <summary>
                        /// 梯度图像压入静态数据流
                        /// <summary>
                        Form1.boduan.Add(rd2);
                        Form1.abl.readmore.Add(rd2);
                        /// <summary>
                        /// availablebandlist中显示梯度数据
                        /// <summary>
                        Form1.abl.PATH = rd2.FileName;
                        Form1.abl.Form_Load(sender, e);
                    }
                    if (listBoxControl2.Text == "Prewitt")
                    {
                        Prewitt_Sobel ps = new Prewitt_Sobel(Form1.boduan[N].ColumnCounts, Form1.boduan[N].LineCounts, Form1.boduan[N].bands);
                        ps.PrePer1(Form1.boduan[N].BandsDataD);
                        ps.PrePer2(Form1.boduan[N].BandsDataD);
                        /// <summary>
                        /// 结果赋值
                        /// <summary>
                        rd = ps.GetGrad1();
                        rd2 = ps.GetGrad2();
                        /// <summary>
                        /// 文件名
                        /// <summary>
                        if (textEdit1.Text == "")
                        {
                            rd.FileName = "PrewittGW";
                            rd2.FileName = "PrewittGV";
                        }
                        else
                        {
                            rd.FileName = textEdit1.Text + "_GW";
                            rd2.FileName = textEdit1.Text + "_GV";
                        }
                        /// <summary>
                        /// 波段名
                        /// <summary>
                        for (int i = 0; i < rd.bands; i++)
                        {
                            rd.Bandsname[i] = "PTGW: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                            rd2.Bandsname[i] = "PTGV: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                        }
                        /// <summary>
                        /// 梯度图像压入静态数据流
                        /// <summary>
                        Form1.boduan.Add(rd2);
                        Form1.abl.readmore.Add(rd2);
                        /// <summary>
                        /// availablebandlist中显示梯度数据
                        /// <summary>
                        Form1.abl.PATH = rd2.FileName;
                        Form1.abl.Form_Load(sender, e);
                    }
                    else if (listBoxControl2.Text == "Sobel")
                    {
                        Prewitt_Sobel ps = new Prewitt_Sobel(Form1.boduan[N].ColumnCounts, Form1.boduan[N].LineCounts, Form1.boduan[N].bands);
                        ps.SobPer1(Form1.boduan[N].BandsDataD);
                        ps.SobPer2(Form1.boduan[N].BandsDataD);
                        /// <summary>
                        /// 结果赋值
                        /// <summary>
                        rd = ps.GetGrad1();
                        rd2 = ps.GetGrad2();
                        /// <summary>
                        /// 文件名
                        /// <summary>
                        if (textEdit1.Text == "")
                        {
                            rd.FileName = "SobelGW";
                            rd2.FileName = "SobelGV";
                        }
                        else
                        {
                            rd.FileName = textEdit1.Text + "_GW";
                            rd2.FileName = textEdit1.Text + "_GV";
                        }
                        /// <summary>
                        /// 波段名
                        /// <summary>
                        for (int i = 0; i < rd.bands; i++)
                        {
                            rd.Bandsname[i] = "SLGW: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                            rd2.Bandsname[i] = "SLGV: " + "(" + Form1.boduan[N].Bandsname[i] + ")";
                        }
                        /// <summary>
                        /// 梯度图像压入静态数据流
                        /// <summary>
                        Form1.boduan.Add(rd2);
                        Form1.abl.readmore.Add(rd2);
                        /// <summary>
                        /// availablebandlist中显示梯度数据
                        /// <summary>
                        Form1.abl.PATH = rd2.FileName;
                        Form1.abl.Form_Load(sender, e);
                    }
                }
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
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 切换选择数据时，改变头文件信息事件
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
        
    }
}