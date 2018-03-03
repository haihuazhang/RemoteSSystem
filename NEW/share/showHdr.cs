using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteSystem
{
    class showHdr
    {
        public RichTextBox richTextBox1 = new RichTextBox();
        public RichTextBox gethdr(string filename)
        {
            GetDataByFilename gdbf = new GetDataByFilename();
            int record = gdbf.getnumber(Form1.boduan, filename);
            richTextBox1.Text += "Name:" + "\t"+Form1.boduan[record].FileName + "\r\n";
            richTextBox1.Text += "Columns:" + "\t" + Form1.boduan[record].ColumnCounts + "\r\n";
            richTextBox1.Text += "Lines:" + "\t" + Form1.boduan[record].LineCounts + "\r\n";
            richTextBox1.Text += "bands:" + "\t" + Form1.boduan[record].bands + "\r\n";
            richTextBox1.Text += "DataType:" + "\t" + Form1.boduan[record].DataType + "\r\n";
            return richTextBox1;
        }
    }
}
