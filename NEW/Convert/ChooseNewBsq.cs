using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RemoteSystem
{
    class ChooseNewBsq
    {
        public byte[] bits;
        /// <summary>
        /// 读取BSQ文件
        /// </summary>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <param name="bands"></param>
        /// <param name="BSQPATH"></param>
        /// <returns></returns>
        public bool BSQAread(int ColumnCounts, int LineCounts, int bands,string BSQPATH)
        {
            bits = new byte[ColumnCounts * LineCounts * bands];
            int DataType = 1;
             int totalnum = ColumnCounts * LineCounts * DataType * bands;//计算文件字节数
            int n = 0;
            
             FileStream fsopen = new FileStream(BSQPATH, FileMode.Open);
                int bt;
                while ((bt = fsopen.ReadByte()) > -1)
                {
                    bits[n] = Convert.ToByte(bt);
                    n++;
                }
           
            if (n != totalnum - 1)
            {

                return false;
            }
            fsopen.Flush();
            fsopen.Close();
            fsopen.Dispose();
            return true;
        }
    }
}
