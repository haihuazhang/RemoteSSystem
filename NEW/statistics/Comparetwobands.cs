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
    public partial class Comparetwobands : Form
    {
        public Comparetwobands()
        {
            InitializeComponent();
        }
        public double average1, average2, standard1, standard2, Covariance, Correlation;
        public int i, j;
        /// <summary>
        /// 显示波段统计量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Comparetwobands_Load(object sender, EventArgs e)
        {
            this.textBox1.Text += "波段" + (i + 1);
            this.textBox1.Text += "\t均值:";
            this.textBox1.Text += average1.ToString();
            this.textBox1.Text += "\t标准差:";
            this.textBox1.Text += standard1.ToString();
            this.textBox1.Text += "\r\n\r\n\r\n";

            this.textBox1.Text += "波段" + (j + 1);
            this.textBox1.Text += "\t均值:";
            this.textBox1.Text += average2.ToString();
            this.textBox1.Text += "\t标准差:";
            this.textBox1.Text += standard2.ToString();
            this.textBox1.Text += "\r\n\r\n\r\n";
            this.textBox1.Text += "协方差:";
            this.textBox1.Text += Covariance.ToString();
            this.textBox1.Text += "\r\n相关系数:";
            this.textBox1.Text += Correlation.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
