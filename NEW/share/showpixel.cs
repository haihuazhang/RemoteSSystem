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
    public partial class showpixel : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public showpixel()
        {
            InitializeComponent();
        }
        public string[] orginData;
        public string[] showData;
        public Label label1= new Label();
        private void showpixel_Load(object sender, EventArgs e)
        {

            this.Width = 200;
            this.Height = 200;
            label1.Dock = DockStyle.Fill;
            this.Location = new Point(MousePosition.X-20,MousePosition.Y+20);
        }

    }
}