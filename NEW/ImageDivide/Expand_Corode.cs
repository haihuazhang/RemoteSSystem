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
    public partial class Expand_Corode : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Expand_Corode()
        {
            InitializeComponent();
        }
        public int[,] kernel;
        /// <summary>
        /// 处理状态，0为腐蚀，1为膨胀,-1为取消当前操作
        /// </summary>
        public int handelState;
        private void Expand_Corode_Load(object sender, EventArgs e)
        {
            kernel = new int[3, 3] { { 0, 0, 0 }, { 1, 1, 0 }, { 0, 1, 0 } };
            handelState = 0;
            richTextBox1.Text = kernel[0, 0].ToString();
            richTextBox2.Text = kernel[0, 1].ToString();
            richTextBox3.Text = kernel[0, 2].ToString();
            richTextBox4.Text = kernel[1, 0].ToString();
            richTextBox5.Text = kernel[1, 1].ToString();
            richTextBox6.Text = kernel[1, 2].ToString();
            richTextBox7.Text = kernel[2, 0].ToString();
            richTextBox8.Text = kernel[2, 1].ToString();
            richTextBox9.Text = kernel[2, 2].ToString();
            labelControl1.Text = "Corrode";
        }


        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            kernel = new int[3, 3] { { 0, 0, 0 }, { 1, 1, 0 }, { 0, 1, 0 } };
            handelState = 0;
            richTextBox1.Text = kernel[0, 0].ToString();
            richTextBox2.Text = kernel[0, 1].ToString();
            richTextBox3.Text = kernel[0, 2].ToString();
            richTextBox4.Text = kernel[1, 0].ToString();
            richTextBox5.Text = kernel[1, 1].ToString();
            richTextBox6.Text = kernel[1, 2].ToString();
            richTextBox7.Text = kernel[2, 0].ToString();
            richTextBox8.Text = kernel[2, 1].ToString();
            richTextBox9.Text = kernel[2, 2].ToString();
            labelControl1.Text = "Corrode";
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            kernel = new int[3, 3] { { 0, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
            handelState = 1;
            richTextBox1.Text = kernel[0, 0].ToString();
            richTextBox2.Text = kernel[0, 1].ToString();
            richTextBox3.Text = kernel[0, 2].ToString();
            richTextBox4.Text = kernel[1, 0].ToString();
            richTextBox5.Text = kernel[1, 1].ToString();
            richTextBox6.Text = kernel[1, 2].ToString();
            richTextBox7.Text = kernel[2, 0].ToString();
            richTextBox8.Text = kernel[2, 1].ToString();
            richTextBox9.Text = kernel[2, 2].ToString();
            labelControl1.Text = "Expand";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            handelState = -1;
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "1" || richTextBox1.Text == "0")
            {
                kernel[0, 0] = Convert.ToInt16(richTextBox1.Text);
            }
            else
            {
                richTextBox1.Text = kernel[0, 0].ToString();
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox2.Text == "1" || richTextBox2.Text == "0")
            {
                kernel[0, 1] = Convert.ToInt16(richTextBox2.Text);
            }
            else
            {
                richTextBox2.Text = kernel[0, 1].ToString();
            }
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox3.Text == "1" || richTextBox3.Text == "0")
            {
                kernel[0, 2] = Convert.ToInt16(richTextBox3.Text);
            }
            else
            {
                richTextBox3.Text = kernel[0, 2].ToString();
            }
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox4.Text == "1" || richTextBox4.Text == "0")
            {
                kernel[1, 0] = Convert.ToInt16(richTextBox4.Text);
            }
            else
            {
                richTextBox4.Text = kernel[1, 0].ToString();
            }
        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox5.Text == "1" || richTextBox5.Text == "0")
            {
                kernel[1, 1] = Convert.ToInt16(richTextBox5.Text);
            }
            else
            {
                richTextBox5.Text = kernel[1, 1].ToString();
            }
        }

        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox6.Text == "1" || richTextBox6.Text == "0")
            {
                kernel[1, 2] = Convert.ToInt16(richTextBox6.Text);
            }
            else
            {
                richTextBox6.Text = kernel[1, 2].ToString();
            }
        }

        private void richTextBox7_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox7.Text == "1" || richTextBox7.Text == "0")
            {
                kernel[2, 0] = Convert.ToInt16(richTextBox7.Text);
            }
            else
            {
                richTextBox7.Text = kernel[2, 0].ToString();
            }
        }

        private void richTextBox8_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox8.Text == "1" || richTextBox8.Text == "0")
            {
                kernel[2, 1] = Convert.ToInt16(richTextBox8.Text);
            }
            else
            {
                richTextBox8.Text = kernel[2, 1].ToString();
            }
        }

        private void richTextBox9_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox9.Text == "1" || richTextBox9.Text == "0")
            {
                kernel[2, 2] = Convert.ToInt16(richTextBox9.Text);
            }
            else
            {
                richTextBox9.Text = kernel[2, 2].ToString();
            }
        }

    }
}