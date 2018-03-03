using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.IO;

namespace RemoteSystem
{
    public partial class AvailableBandsList : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        /// <summary>
        /// AvailableBandsList窗体，实现遥感数据概要显示（树状图显示），以及为遥感数据选择RGB波段合成或灰度显示
        /// </summary>
        public AvailableBandsList()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 为新增数据时候提供索引
        /// </summary>
        public string PATH = "";
        /// <summary>
        /// 该窗体数据流
        /// </summary>
        public List<read> readmore = new List<read>();
        /// <summary>
        /// imageview窗口数量
        /// </summary>
        //public int  windowsnum;
        /// <summary>
        /// imageview窗口泛型
        /// </summary>
        public List<imageview> Wins = new List<imageview>();
        /// <summary>
        /// 增加节点（文件名为父节点，波段名为子节点）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Form_Load(object sender, EventArgs e)
        {
            TreeNode root = new TreeNode();
            root.Text = Path.GetFileName(PATH);
            treeView1.Nodes.Add(root);
            //获取单图像数据编号
            GetDataByFilename gdb = new GetDataByFilename();
            int time = gdb.getnumber(readmore, root.Text);
            for (int i = 0; i < readmore[time].bands; i++)
            {
                TreeNode leave = new TreeNode();
                leave.Text = readmore[time].Bandsname[i];
                root.Nodes.Add(leave);
            }
            panel1.Show();
            radioButton1.Checked = true;
            panel2.Hide();
            panel2.Location = panel1.Location;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// No Display 时创建imageview新实例
            /// <summary>
            if (comboBox1.Text == "No Display")
            {
                /// <summary>
                /// comboBox操作，类似envi
                /// <summary>
                //windowsnum++;
                comboBox1.Items.Add("Display #" + 1);
                comboBox1.SelectedItem = "Display #" + 1;
                comboBox1.Items.Remove("No Display");
                /// <summary>
                /// 定义新窗体
                /// <summary>
                imageview imv = new imageview("Display #" + 1);
                imv.Text = "Display #" + 1;
                /// <summary>
                /// 选择灰度(单波段)显示
                /// <summary>
                if (radioButton1.Checked)
                {
                    if (textBox1.Text != "")
                    {
                        /// <summary>
                        ///索引数据流，得到数据
                        /// <summary>
                        string pathmark = Path.GetFileName(textBox1.Text);

                        GetDataByFilename gdb = new GetDataByFilename();
                        int i = gdb.getnumber(readmore, pathmark);
                        GetBandByname gbn = new GetBandByname();
                        int single = gbn.getnumber(readmore[i].Bandsname, this.textBox1.Text.Substring(0, this.textBox1.Text.IndexOf(pathmark) - 1)
                            , readmore[i].bands);
                        /// <summary>
                        /// imv实例中各项数据初始化
                        /// <summary>
                        imv.ColumnCounts = readmore[i].ColumnCounts;
                        imv.LineCounts = readmore[i].LineCounts;
                        imv.bands = 1;
                        imv.showdata = new int[imv.bands, imv.ColumnCounts * imv.LineCounts];
                        imv.BandsDataD = new double[imv.bands, imv.ColumnCounts * imv.LineCounts];
                        imv.bandstemp = new int[imv.bands, imv.ColumnCounts * imv.LineCounts];
                        /// <summary>
                        ///原数据存储
                        /// <summary>
                        for (int j = 0; j < readmore[i].ColumnCounts * readmore[i].LineCounts; j++)
                            imv.BandsDataD[0, j] = readmore[i].BandsDataD[single, j];
                        imv.pictureBox1.Height = imv.LineCounts;
                        imv.pictureBox1.Width = imv.ColumnCounts;

                        ///窗口状态初始化
                        imv.handleBopen();
                        /// <summary>
                        ///通过LinearShow方法，将原数据拉伸至0-255用于显示
                        /// <summary>
                        imagestretch images = new imagestretch(imv.ColumnCounts, imv.LineCounts, imv.bands);
                        //images.LinearShow(imv.showdata, imv.BandsDataD);
                        imv.showdata = images.TwoPercentStretch(imv.BandsDataD);
                        /// <summary>
                        ///窗口命名
                        /// <summary>
                        imv.Text = textBox1.Text;
                        /// <summary>
                        ///显示图像
                        /// <summary>
                        ImageShow ims = new ImageShow();
                        imv.pictureBox1.Refresh();
                        Bitmap map = new Bitmap(imv.pictureBox1.Width, imv.pictureBox1.Height);

                        ims.showimage(imv.showdata, imv.ColumnCounts, imv.LineCounts, imv.bands, map);

                        imv.pictureBox1.Image = map;
                        
                        imv.Show();
                        imv.Visible = true;
                    }
                    else
                        MessageBox.Show("请选择波段！");

                }
                else if (radioButton2.Checked)
                {
                    if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
                    {
                        /// <summary>
                        ///从list数据流中按文件名索引数据。
                        /// <summary>
                        string pathmark2 = Path.GetFileName(textBox2.Text);
                        string pathmark3 = Path.GetFileName(textBox3.Text);
                        string pathmark4 = Path.GetFileName(textBox4.Text);
                        GetDataByFilename gdb = new GetDataByFilename();
                        int i2 = gdb.getnumber(readmore, pathmark2);
                        int i3 = gdb.getnumber(readmore, pathmark3);
                        int i4 = gdb.getnumber(readmore, pathmark4);
                        /// <summary>
                        /// 按波段名索引波段号
                        /// <summary>
                        GetBandByname gbn = new GetBandByname();
                        int Rband = gbn.getnumber(readmore[i2].Bandsname, this.textBox2.Text.Substring(0, this.textBox2.Text.IndexOf(pathmark2) - 1)
                            , readmore[i2].bands);
                        int Gband = gbn.getnumber(readmore[i3].Bandsname, this.textBox3.Text.Substring(0, this.textBox3.Text.IndexOf(pathmark3) - 1)
                           , readmore[i3].bands);
                        int Bband = gbn.getnumber(readmore[i4].Bandsname, this.textBox4.Text.Substring(0, this.textBox4.Text.IndexOf(pathmark4) - 1)
                           , readmore[i4].bands);
                        if (Form1.boduan[i2].ColumnCounts == Form1.boduan[i3].ColumnCounts
                            && Form1.boduan[i2].ColumnCounts == Form1.boduan[i4].ColumnCounts
                            && Form1.boduan[i2].LineCounts == Form1.boduan[i3].LineCounts
                            && Form1.boduan[i2].LineCounts == Form1.boduan[i4].LineCounts)
                        {
                            /// <summary>
                            /// imv实例中各项数据初始化
                            /// <summary>
                            imv.LineCounts = readmore[i2].LineCounts;
                            imv.ColumnCounts = readmore[i3].ColumnCounts;
                            imv.bands = 3;
                            imv.showdata = new int[imv.bands, imv.LineCounts * imv.ColumnCounts];
                            imv.BandsDataD = new double[imv.bands, imv.ColumnCounts * imv.LineCounts];
                            /// <summary>
                            ///任意图像任意波段合成（数据传递）
                            /// <summary>
                            for (int j = 0; j < imv.ColumnCounts * imv.LineCounts; j++)
                                imv.BandsDataD[0, j] = readmore[i2].BandsDataD[Rband, j];
                            for (int j = 0; j < imv.ColumnCounts * imv.LineCounts; j++)
                                imv.BandsDataD[1, j] = readmore[i3].BandsDataD[Gband, j];
                            for (int j = 0; j < imv.ColumnCounts * imv.LineCounts; j++)
                                imv.BandsDataD[2, j] = readmore[i4].BandsDataD[Bband, j];

                            imv.pictureBox1.Height = imv.LineCounts;
                            imv.pictureBox1.Width = imv.ColumnCounts;
                            ///窗口状态初始化
                            imv.handleBopen();
                            /// <summary>
                            ///通过LinearShow方法，将原数据拉伸至0-255用于显示
                            /// <summary>
                            imagestretch images = new imagestretch(imv.ColumnCounts, imv.LineCounts, imv.bands);
                            images.LinearShow(imv.showdata, imv.BandsDataD);
                            //imv.showdata = images.TwoPercentStretch(imv.BandsDataD);
                            
                            /// <summary>
                            ///窗口命名
                            /// <summary>
                            imv.Text = textBox2.Text + "&" + textBox3.Text + "&" + textBox4.Text;
                            /// <summary>
                            ///显示图像
                            /// <summary>
                            imv.pictureBox1.Refresh();
                            ImageShow ims = new ImageShow();
                            Bitmap map = new Bitmap(imv.pictureBox1.Width, imv.pictureBox1.Height);
                            ims.showimage(imv.showdata, imv.ColumnCounts, imv.LineCounts, imv.bands, map);


                            imv.pictureBox1.Image = map;
                            
                            imv.Show();
                            imv.Visible = true;
                        }
                        else
                            MessageBox.Show("波段规格不匹配！");

                    }
                    else
                        MessageBox.Show("请输入波段！");

                }
                /// <summary>
                /// imv实例加入Wins泛型中
                /// <summary>
                Wins.Add(imv);
            }
            /// <summary>
            ///对已存在的imageview窗体进行数据传递和操作
            /// <summary>
            else
            {
                /// <summary>
                /// 搜索窗口
                /// <summary>
                GetWinByName gwn = new GetWinByName();
                int WinNbr = gwn.GetWinN(Wins, comboBox1.Text);

                if (radioButton1.Checked)
                {
                    if (textBox1.Text != "")
                    {
                        string pathmark = Path.GetFileName(textBox1.Text);
                        /// <summary>
                        ///索引数据流，得到数据
                        /// <summary>
                        GetDataByFilename gdb = new GetDataByFilename();
                        int i = gdb.getnumber(readmore, pathmark);
                        GetBandByname gbn = new GetBandByname();
                        int single = gbn.getnumber(readmore[i].Bandsname, this.textBox1.Text.Substring(0, this.textBox1.Text.IndexOf(pathmark) - 1)
                            , readmore[i].bands);
                        /// <summary>
                        /// imv实例中各项数据初始化
                        /// <summary>
                        Wins[WinNbr].ColumnCounts = readmore[i].ColumnCounts;
                        Wins[WinNbr].LineCounts = readmore[i].LineCounts;
                        Wins[WinNbr].bands = 1;
                        Wins[WinNbr].showdata = new int[Wins[WinNbr].bands, Wins[WinNbr].ColumnCounts * Wins[WinNbr].LineCounts];
                        Wins[WinNbr].BandsDataD = new double[Wins[WinNbr].bands, Wins[WinNbr].ColumnCounts * Wins[WinNbr].LineCounts];
                        /// <summary>
                        ///原数据存储
                        /// <summary>
                        for (int j = 0; j < readmore[i].ColumnCounts * readmore[i].LineCounts; j++)
                            Wins[WinNbr].BandsDataD[0, j] = readmore[i].BandsDataD[single, j];

                        Wins[WinNbr].pictureBox1.Height = Wins[WinNbr].LineCounts;
                        Wins[WinNbr].pictureBox1.Width = Wins[WinNbr].ColumnCounts;

                        ///窗口状态初始化
                        Wins[WinNbr].handleBopen();
                        /// <summary>
                        ///通过LinearShow方法，将原数据拉伸至0-255用于显示
                        /// <summary>
                        imagestretch images = new imagestretch(Wins[WinNbr].ColumnCounts, Wins[WinNbr].LineCounts, Wins[WinNbr].bands);
                        //images.LinearShow(Wins[WinNbr].showdata, Wins[WinNbr].BandsDataD);
                        Wins[WinNbr].showdata = images.TwoPercentStretch(Wins[WinNbr].BandsDataD);
                        /// <summary>
                        /// 图像拉伸数据初始化和赋值
                        /// <summary>
                        BandpixelInitialize bpi = new BandpixelInitialize();
                        bpi.Initialization(out Wins[WinNbr].bandstemp, Wins[WinNbr].showdata, Wins[WinNbr].ColumnCounts
                            , Wins[WinNbr].LineCounts, Wins[WinNbr].bands);
                        /// <summary>
                        ///窗口命名
                        /// <summary>
                        Wins[WinNbr].Text = textBox1.Text;
                        /// <summary>
                        ///显示图像
                        /// <summary>
                        ImageShow ims = new ImageShow();
                        //ims.showimage(bandints,ColumnCounts,LineCounts)
                        Wins[WinNbr].pictureBox1.Refresh();
                        Bitmap map = new Bitmap(Wins[WinNbr].pictureBox1.Width, Wins[WinNbr].pictureBox1.Height);

                        ims.showimage(Wins[WinNbr].showdata, Wins[WinNbr].ColumnCounts, Wins[WinNbr].LineCounts, Wins[WinNbr].bands, map);

                        Wins[WinNbr].pictureBox1.Image = map;
                        
                        Wins[WinNbr].Show();
                        Wins[WinNbr].Visible = true;
                    }
                    else
                        MessageBox.Show("请输入波段！");

                }
                else if (radioButton2.Checked)
                {
                    if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
                    {
                        /// <summary>
                        ///从list数据流中按文件名索引数据。
                        /// <summary>
                        string pathmark2 = Path.GetFileName(textBox2.Text);
                        string pathmark3 = Path.GetFileName(textBox3.Text);
                        string pathmark4 = Path.GetFileName(textBox4.Text);
                        GetDataByFilename gdb = new GetDataByFilename();
                        int i2 = gdb.getnumber(readmore, pathmark2);
                        int i3 = gdb.getnumber(readmore, pathmark3);
                        int i4 = gdb.getnumber(readmore, pathmark4);
                        /// <summary>
                        /// 按波段名索引波段号
                        /// <summary>
                        GetBandByname gbn = new GetBandByname();
                        int Rband = gbn.getnumber(readmore[i2].Bandsname, this.textBox2.Text.Substring(0, this.textBox2.Text.IndexOf(pathmark2) - 1)
                            , readmore[i2].bands);
                        int Gband = gbn.getnumber(readmore[i3].Bandsname, this.textBox3.Text.Substring(0, this.textBox3.Text.IndexOf(pathmark3) - 1)
                           , readmore[i3].bands);
                        int Bband = gbn.getnumber(readmore[i4].Bandsname, this.textBox4.Text.Substring(0, this.textBox4.Text.IndexOf(pathmark4) - 1)
                           , readmore[i4].bands);
                        if (Form1.boduan[i2].ColumnCounts == Form1.boduan[i3].ColumnCounts
                          && Form1.boduan[i2].ColumnCounts == Form1.boduan[i4].ColumnCounts
                          && Form1.boduan[i2].LineCounts == Form1.boduan[i3].LineCounts
                          && Form1.boduan[i2].LineCounts == Form1.boduan[i4].LineCounts)
                        {
                            /// <summary>
                            /// imv实例中各项数据初始化
                            /// <summary>
                            Wins[WinNbr].LineCounts = readmore[i2].LineCounts;
                            Wins[WinNbr].ColumnCounts = readmore[i3].ColumnCounts;
                            Wins[WinNbr].bands = 3;
                            Wins[WinNbr].showdata = new int[3, Wins[WinNbr].LineCounts * Wins[WinNbr].ColumnCounts];
                            Wins[WinNbr].BandsDataD = new double[3, Wins[WinNbr].LineCounts * Wins[WinNbr].ColumnCounts];
                            /// <summary>
                            ///任意图像任意波段合成（数据传递）
                            /// <summary>
                            for (int j = 0; j < Wins[WinNbr].ColumnCounts * Wins[WinNbr].LineCounts; j++)
                                Wins[WinNbr].BandsDataD[0, j] = readmore[i2].BandsDataD[Rband, j];
                            for (int j = 0; j < Wins[WinNbr].ColumnCounts * Wins[WinNbr].LineCounts; j++)
                                Wins[WinNbr].BandsDataD[1, j] = readmore[i3].BandsDataD[Gband, j];
                            for (int j = 0; j < Wins[WinNbr].ColumnCounts * Wins[WinNbr].LineCounts; j++)
                                Wins[WinNbr].BandsDataD[2, j] = readmore[i4].BandsDataD[Bband, j];

                            Wins[WinNbr].pictureBox1.Height = Wins[WinNbr].LineCounts;
                            Wins[WinNbr].pictureBox1.Width = Wins[WinNbr].ColumnCounts;
                            ///窗口状态初始化
                            Wins[WinNbr].handleBopen();
                            /// <summary>
                            ///通过LinearShow方法，将原数据拉伸至0-255用于显示
                            /// <summary>
                            imagestretch images = new imagestretch(Wins[WinNbr].ColumnCounts, Wins[WinNbr].LineCounts, Wins[WinNbr].bands);
                            //images.LinearShow(Wins[WinNbr].showdata, Wins[WinNbr].BandsDataD);
                            Wins[WinNbr].showdata = images.TwoPercentStretch(Wins[WinNbr].BandsDataD);
                            /// <summary>
                            /// 图像拉伸数据初始化和赋值
                            /// <summary>
                            BandpixelInitialize bpi = new BandpixelInitialize();
                            bpi.Initialization(out Wins[WinNbr].bandstemp, Wins[WinNbr].showdata, Wins[WinNbr].ColumnCounts
                                , Wins[WinNbr].LineCounts, Wins[WinNbr].bands);
                            /// <summary>
                            ///窗口命名
                            /// <summary>
                            Wins[WinNbr].Text = textBox2.Text + "&" + textBox3.Text + "&" + textBox4.Text;
                            /// <summary>
                            ///显示图像
                            /// <summary>
                            Wins[WinNbr].pictureBox1.Refresh();
                            ImageShow ims = new ImageShow();
                            Bitmap map = new Bitmap(Wins[WinNbr].pictureBox1.Width, Wins[WinNbr].pictureBox1.Height);
                            ims.showimage(Wins[WinNbr].showdata, Wins[WinNbr].ColumnCounts, Wins[WinNbr].LineCounts, Wins[WinNbr].bands, map);


                            Wins[WinNbr].pictureBox1.Image = map;
                            
                            Wins[WinNbr].Show();
                            Wins[WinNbr].Visible = true;
                        }
                        else
                            MessageBox.Show("波段规格不匹配！");
                    }
                    else
                        MessageBox.Show("请输入波段！");
                }
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
       
        }
        /// <summary>
        /// comboBox1选择new display 选项时候 实现imageview窗体增加，并将该实例加入Wins泛型中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "New Display")
            {

                /// <summary>
                /// 得到合适窗口名
                /// <summary>
                string WinName = "";
                /// <summary>
                /// 最大窗口名
                /// <summary>
                int max = 0;
                /// <summary>
                /// 待建窗口序号
                /// <summary>
                int flag = 0;
                /// <summary>
                /// 得到最大窗口名
                /// <summary>
                for (int i = 1; i < comboBox1.Items.Count; i++)
                {
                    string winname = comboBox1.Items[i].ToString();
                    if (Convert.ToInt32(winname.Substring
                        (winname.IndexOf("#") + 1, winname.Length - winname.IndexOf("#") - 1)) > max)
                        max = Convert.ToInt32(winname.Substring
                        (winname.IndexOf("#") + 1, winname.Length - winname.IndexOf("#") - 1));
                }
                /// <summary>
                /// 得到待建窗口序号（考虑到窗口名不连续情况）
                /// <summary>
                for (int i = 1; i <= max + 1; i++)
                {
                    string temp = "Display #" + i;
                    if (!comboBox1.Items.Contains(temp))
                    {
                        flag = i;
                        WinName = temp;
                        break;
                    }
                }
                /// <summary>
                /// 插入合适位置
                /// <summary>
                comboBox1.Items.Insert(flag, WinName);
                comboBox1.SelectedItem = WinName;
                /// <summary>
                /// 新建窗口实例,插入合适位置
                /// <summary>
                imageview imv = new imageview(WinName);
                imv.Text = WinName;
                Wins.Insert(flag - 1, imv);
                imv.Show();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (treeView1.SelectedNode.Parent != null)
            {
                if (radioButton1.Checked)
                {
                    textBox1.Text = treeView1.SelectedNode.Text + ":" + treeView1.SelectedNode.Parent.Text;
                }
                else if (radioButton2.Checked)
                {
                    if (radioButton3.Checked)
                    {
                        textBox2.Text = treeView1.SelectedNode.Text + ":" + treeView1.SelectedNode.Parent.Text;
                        radioButton3.Checked = false;
                        radioButton4.Checked = true;
                    }
                    else if (radioButton4.Checked)
                    {
                        textBox3.Text = treeView1.SelectedNode.Text + ":" + treeView1.SelectedNode.Parent.Text;
                        radioButton4.Checked = false;
                        radioButton5.Checked = true;
                    }
                    else
                    {
                        textBox4.Text = treeView1.SelectedNode.Text + ":" + treeView1.SelectedNode.Parent.Text;
                        radioButton5.Checked = false;
                        radioButton3.Checked = true;
                    }
                }
                treeView1.SelectedNode = null;
            }
        }
        /// <summary>
        /// panel1和panel2 容器的显示与隐藏，以实现RGB合成和灰度合成功能切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Show();
            panel2.Hide();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Show();
            panel1.Hide();
            radioButton3.Checked = true;
        }

        private void AvailableBandsList_Load(object sender, EventArgs e)
        {

        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();

            //base.OnClosing(e);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Parent == null)
            {

                        GetDataByFilename gdbf = new GetDataByFilename();
                        int i = gdbf.getnumber(Form1.boduan, treeView1.SelectedNode.Text);
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = null;
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            SaveFile sf = new SaveFile(Form1.boduan[i], sfd.FileName, Form1.boduan[i].DataType);
                            sf.SaveDataF(); sf.Savehdr();
                            
                        }

                }
            }
    
        private void Close_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Parent != null)
            {
                MessageBox.Show("请选中父节点！");
                return;
            }
            if (treeView1.SelectedNode.Parent == null)
            {
                DialogResult ReturnDlg = MessageBox.Show(this, "Be sure to Close Selected File？", "信息提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                switch (ReturnDlg)
                {
                    case DialogResult.OK:
                        GetDataByFilename gdbf =new GetDataByFilename();
                        int i =gdbf.getnumber(Form1.boduan,treeView1.SelectedNode.Text);
                        Form1.boduan.RemoveAt(i);
                        this.readmore.RemoveAt(i);
                        this.treeView1.Nodes.Remove(treeView1.SelectedNode);
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
        }
        
    }
}