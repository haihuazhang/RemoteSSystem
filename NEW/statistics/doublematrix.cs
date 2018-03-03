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
    public partial class doublematrix : Form
    {
        public doublematrix()
        {
            InitializeComponent();
        }
        public int[,] doublepixel;
        /// <summary>
        /// 显示共生矩阵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doublematrix_Load(object sender, EventArgs e)
        {
            this.dataGridView1.ColumnCount = 256;
            this.dataGridView1.RowCount = 256;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    this.dataGridView1[j, i].Value = this.doublepixel[i, j].ToString();
                }
            }
        }

        
    }
}
