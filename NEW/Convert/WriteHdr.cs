using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RemoteSystem
{
    /// <summary>
    /// 将近弃用
    /// </summary>
    class WriteHdr
    {
        string Content = "";
        public void BSQHDR(string BSQhdrPath)
        {
            
            StreamReader sr = new StreamReader("..//AA.hdr");
            StreamWriter sw = new StreamWriter(BSQhdrPath);
            while ((Content = sr.ReadLine()) != null)
            {
                sw.WriteLine(Content);
            }
            sr.Close();
            sr.Dispose();
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        public void BILHDR(string BILhdrPath)
        {
            StreamReader sr = new StreamReader("..//AA.hdr");
            StreamWriter sw = new StreamWriter(BILhdrPath);
            while ((Content = sr.ReadLine()) != null)
            {
                if (Content.IndexOf("interleave") > -1)
                {
                    Content = "interleave = bil";
                }
                sw.WriteLine(Content);
            }
            sr.Close();
            sr.Dispose();
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        public void BIPHDR(string BIPhdrPath)
        {
            StreamReader sr = new StreamReader("..//AA.hdr");
            StreamWriter sw = new StreamWriter(BIPhdrPath);
            while ((Content = sr.ReadLine()) != null)
            {
                if (Content.IndexOf("interleave") > -1)//修改文件存储格式
                {
                    Content = "interleave = bip";
                }
                sw.WriteLine(Content);
            }
            sr.Close();
            sr.Dispose();
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }


            

            

    }
}
