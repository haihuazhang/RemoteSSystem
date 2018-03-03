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
    public partial class unionhistogram : Form
    {
        public int[,] union = new int[256, 256];
        public unionhistogram()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 联合直方图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form3_Load(object sender, EventArgs e)
        {
            dataGridView1.RowCount = 256;
            dataGridView1.ColumnCount = 256;
            for (int i = 0; i <256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    this.dataGridView1[j,i].Value = this.union[i, j].ToString();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
