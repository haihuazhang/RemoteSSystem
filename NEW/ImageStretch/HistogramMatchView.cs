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
    /// 直方图匹配选择窗口
    /// </summary>
    public partial class HistogramMatch : Form
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public HistogramMatch()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 匹配窗口
        /// </summary>
        public int Winnumber2=-9999 ;
        /// <summary>
        /// 执行直方图匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                /// <summary>
                /// 确定目标窗口
                /// <summary>
                Winnumber2 = Convert.ToInt32(listBox1.SelectedItem.ToString().Substring
                    (listBox1.SelectedItem.ToString().Length - 1))-1;
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择目标窗口！");
            }
        }
        /// <summary>
        /// 关闭该窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HistogramMatch_Load(object sender, EventArgs e)
        {

        }

    }
}
