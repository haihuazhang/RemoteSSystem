using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Data;
using System.Text;

namespace RemoteSystem
{
    public class BSQ
    {
        public int ColumnCounts, LineCounts, DataType, bands;
        public string Interleave;
        public byte[] bits;
        public int totalnum;

        public bool HDRread(string PATHstr)
        {
            ColumnCounts = 0;
            LineCounts = 0;
            DataType = 0;
            Interleave = "";
            string Content = "";
            bands = 0;
            StreamReader sr = new StreamReader(PATHstr);
            try
            {

                
                while ((Content = sr.ReadLine()) != null)
                {
                    if (Content.IndexOf("samples") > -1)
                    {
                        ColumnCounts = Convert.ToInt32(Content.Trim().Substring(Content.Trim().IndexOf("=") + 1, Content.Trim().Length - Content.Trim().IndexOf("=") - 1));
                    }
                    else if (Content.IndexOf("lines") > -1)
                    {
                        LineCounts = Convert.ToInt32(Content.Trim().Substring(Content.Trim().IndexOf("=") + 1, Content.Trim().Length - Content.Trim().IndexOf("=") - 1));
                    } 
                    else if (Content.IndexOf("bands") > -1)
                    {
                        bands = Convert.ToInt32(Content.Trim().Substring(Content.Trim().IndexOf("=") + 1, Content.Trim().Length - Content.Trim().IndexOf("=") - 1));
                    }
                    else if (Content.IndexOf("data type") > -1)
                    {
                        DataType = Convert.ToInt32(Content.Trim().Substring(Content.Trim().IndexOf("=") + 1, Content.Trim().Length - Content.Trim().IndexOf("=") - 1));
                    }
                    else if (Content.IndexOf("interleave") > -1)
                    {
                        Interleave = Content.Trim().Substring(Content.Trim().IndexOf("=") + 1, Content.Trim().Length - Content.Trim().IndexOf("=") - 1);
                        
                    }
                }
            }
            catch
            {
                sr.Close();
                sr.Dispose();
                return false;
            }
            sr.Close();
            sr.Dispose();
            return true;
        }
        public bool BSQread(string[] BsqPath)
        {
            bits = new byte[ColumnCounts * LineCounts * bands];
             totalnum = ColumnCounts * LineCounts * DataType * bands;//计算文件字节数
            int n = 0;
            for (int i = 0; i < 7; i++)
            {
                FileStream fsopen = new FileStream(BsqPath[i],FileMode.Open);
                int bt;
                while ((bt = fsopen.ReadByte()) >-1)
                {
                    bits[n] = Convert.ToByte(bt);
                    n++;
                }
                fsopen.Flush();
                fsopen.Close();
                fsopen.Dispose();
            }
            if (n != totalnum - 1)
            {
                
                return false;
            }
            return true;
            
        }













       
    }
}
