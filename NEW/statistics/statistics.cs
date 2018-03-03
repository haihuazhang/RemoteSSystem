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
    public partial class statistics : Form
    {
        public statistics(int ColumnCounts, int LineCounts, int bands)
        {
            InitializeComponent();
            BandsDataD = new double[bands, ColumnCounts * LineCounts];
            showdata = new int[bands, ColumnCounts * LineCounts];
        }
        //public double standard1, standard2, Covariance, Correlation, average1, average2;
        /// <summary>
        /// double型数据用于统计值计算
        /// </summary>
        public double[,] BandsDataD;
        /// <summary>
        /// int型数据（0-255值域）用于联合直方图 
        /// </summary>
        public int[,] showdata;
        //public int counts;
        public int ColumnCounts, LineCounts, bands;
        /// <summary>
        /// 联合直方图二维数组（[255,255]）
        /// </summary>
        public int[,] union;
        /// <summary>
        /// 波段像素平均数
        /// </summary>
        public double[] average;
        /// <summary>
        /// 波段像素标准差
        /// </summary>
        public double[] standard;
        /// <summary>
        /// 波段协方差
        /// </summary>
        public double[,] Covariance;
        /// <summary>
        /// 波段相关系数
        /// </summary>
        public double[,] Correlation;

        private void Form4_Load(object sender, EventArgs e)
        {
            for (int h = 1; h <= bands; h++)
            {

                comboBox1.Items.Add("band" + h);
                comboBox2.Items.Add("band" + h);
            }
            average = new double[bands];
            standard = new double[bands];
            Covariance = new double[bands, bands];
            Correlation = new double[bands, bands];
            double total1 = 0; double total2 = 0;
            for (int i = 0; i < bands; i++)
            {
                for (int k = 0; k < this.ColumnCounts * this.LineCounts; k++)
                {
                    total1 += BandsDataD[i, k];

                }
                average[i] = total1 / Convert.ToDouble(this.ColumnCounts * this.LineCounts);
                total1 = 0;
                for (int k = 0; k < this.ColumnCounts * this.LineCounts; k++)
                {
                    total1 += Math.Pow(BandsDataD[i, k] - average[i], Convert.ToDouble(2));

                }
                standard[i] = Math.Sqrt(total1 / Convert.ToDouble(this.ColumnCounts * this.LineCounts));
                total1 = 0;
            }
            for (int i = 0; i < bands; i++)
            {
                for (int j = 0; j < bands; j++)
                {
                    for (int k = 0; k < this.ColumnCounts * this.LineCounts; k++)
                    {
                        total2 += (BandsDataD[i, k] - average[i]) * (BandsDataD[j, k] - average[j]);
                    }
                    Covariance[i, j] = total2 / Convert.ToDouble(this.ColumnCounts * this.LineCounts);
                    Correlation[i, j] = Covariance[i, j] / (standard[i] * standard[j]);
                    total2 = 0;
                }
            }
            for (int i = 0; i < bands; i++)
            {
                for (int j = 0; j < bands; j++)
                {
                    Correlation[i, j] = Math.Round(Correlation[i, j], 6);
                    Covariance[i, j] = Math.Round(Covariance[i, j], 6);
                }
                standard[i] = Math.Round(standard[i], 6);
                average[i] = Math.Round(average[i], 6);
            }
            this.textBox1.Text += "\t\t";
            for (int i = 0; i < bands; i++)
            {
                this.textBox1.Text += "band" + (i + 1);
                this.textBox1.Text += "\t\t";
            }
            this.textBox1.Text += "\r\n";
            this.textBox1.Text += "average:\t";
            for (int i = 0; i < bands; i++)
            {
                this.textBox1.Text += average[i];
                this.textBox1.Text += "\t";
            }
            this.textBox1.Text += "\r\n";
            this.textBox1.Text += "standard:\t";
            for (int i = 0; i < bands; i++)
            {
                this.textBox1.Text += standard[i];
                this.textBox1.Text += "\t";
            }
            this.textBox1.Text += "\r\n\r\n";
            this.textBox1.Text += "Covariance";
            this.textBox1.Text += "\r\n";
            this.textBox1.Text += "\t";
            for (int i = 0; i < bands; i++)
            {
                this.textBox1.Text += "band" + (i + 1);
                this.textBox1.Text += "\t\t";
            }
            this.textBox1.Text += "\r\n";
            for (int i = 0; i < bands; i++)
            {
                this.textBox1.Text += "band" + (i + 1);
                this.textBox1.Text += "\t";
                for (int j = 0; j < bands; j++)
                {
                    this.textBox1.Text += Covariance[i, j].ToString("#0.000000");
                    this.textBox1.Text += "\t";
                }
                this.textBox1.Text += "\r\n";
            }


            this.textBox1.Text += "\r\n\r\n";
            this.textBox1.Text += " Correlation";
            this.textBox1.Text += "\r\n";
            this.textBox1.Text += "\t";
            for (int i = 0; i < bands; i++)
            {
                this.textBox1.Text += "band" + (i + 1);
                this.textBox1.Text += "\t\t";
            }
            this.textBox1.Text += "\r\n";
            for (int i = 0; i < bands; i++)
            {
                this.textBox1.Text += "band" + (i + 1);
                this.textBox1.Text += "\t";
                for (int j = 0; j < bands; j++)
                {
                    this.textBox1.Text += Correlation[i, j].ToString("#0.000000");
                    this.textBox1.Text += "\t";
                }
                this.textBox1.Text += "\r\n";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(comboBox1.Text.Substring(4)) - 1;

            int j = Convert.ToInt32(comboBox2.Text.Substring(4)) - 1;

            union = new int[256, 256];
            for (int k = 0; k < ColumnCounts * LineCounts; k++)
            {
                union[showdata[i, k], showdata[j, k]]++;

            }

            unionhistogram un = new unionhistogram();
            un.union = this.union;
            un.Show();
        }
        /// <summary>
        /// 两波段对比显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //double average1, average2; //平均值
            //double total1 = 0; double total2 = 0;
            //double Covariance;//协方差
            //double Correlation;//相关系数
            //double standard1, standard2;//标准差
            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("请选择两个波段！");
                return;
            }
            int i = Convert.ToInt32(comboBox1.Text.Substring(4)) - 1;

            int j = Convert.ToInt32(comboBox2.Text.Substring(4)) - 1;
            Comparetwobands ctb = new Comparetwobands();
            ctb.i = i;
            ctb.j = j;
            ctb.average1 = this.average[i];
            ctb.average2 = this.average[j];
            ctb.standard1 = this.standard[i];
            ctb.standard2 = this.standard[j];
            ctb.Covariance = this.Covariance[i, j];
            ctb.Correlation = this.Correlation[i, j];
            ctb.Show();



            //private void textBox1_TextChanged(object sender, EventArgs e)
            //{

            //}
        }
    }
}
