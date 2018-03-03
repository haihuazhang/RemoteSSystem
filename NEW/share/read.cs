using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RemoteSystem
{
    public class read
    {
        /// <summary>
        /// 可调用变量
        /// </summary>
        public int ColumnCounts, LineCounts, DataType, bands;
        public string Interleave;
        public int totalnum;
        public byte[] bits  ;
        public double[] Datatemp;
        public int[,] BandsData;//整形数据。
        public string FileName;//文件名作为索引号
        public double[,] BandsDataD;
        public string[] Bandsname;
        public string SensorType;
        /// <summary>
        /// 读取头文件
        /// </summary>
        /// <param name="HdrPath"> 读取文件头文件信息</param>
        /// <returns>头文件读取是否成功</returns>
        public bool HDRread(string HdrPath)
        {
            ColumnCounts = 0;
            LineCounts = 0;
            DataType = 0;
            Interleave = "";
            string Content = "";
            bands = 0;
            //判断头文件中是否有band names
            bool bandnames = false;
            StreamReader sr = new StreamReader(HdrPath);
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
                        Bandsname = new string[bands];
                    }
                    else if (Content.IndexOf("data type") > -1)
                    {
                        DataType = Convert.ToInt32(Content.Trim().Substring(Content.Trim().IndexOf("=") + 1, Content.Trim().Length - Content.Trim().IndexOf("=") - 1));
                    }
                    else if (Content.IndexOf("interleave") > -1)
                    {
                        Interleave = Content.Trim().Substring(Content.Trim().IndexOf("=") + 1, Content.Trim().Length - Content.Trim().IndexOf("=") - 1);

                    }
                    else if (Content.IndexOf("sensor type") > -1)
                    {
                        SensorType = Content.Trim().Substring(Content.Trim().IndexOf("=") + 1, Content.Trim().Length - Content.Trim().IndexOf("=") - 1);
                    }
                    else if (Content.IndexOf("band names") > -1)
                    {
                        Content = sr.ReadLine();
                        Content = Content.Substring(1, Content.IndexOf("}") - 1);
                        for (int i = 0; i < bands; i++)
                            Bandsname[i] = Content.Split(',')[i].Trim();
                        bandnames = true;
                    }
                    else
                    {
                        if (!bandnames)
                            for (int i = 0; i < bands; i++)
                                Bandsname[i] = "band" + (i + 1);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataPath">数据文件路径</param>
        /// <returns>数据文件读取是否成功</returns>
        public bool Dataread(string DataPath)
        {
            totalnum = ColumnCounts * LineCounts * DataType * bands;//计算文件字节数
            int n = 0;
            FileName = Path.GetFileName(DataPath);
            bits = new byte[ColumnCounts * LineCounts * bands*DataType];
            FileStream fsopen = new FileStream(DataPath, FileMode.Open);
            int bt;
            while ((bt = fsopen.ReadByte()) > -1)
            {
                bits[n] = Convert.ToByte(bt);
                n++;
            }
            fsopen.Close();
            fsopen.Dispose();
            if (BandHandle())
            {
                for (int i = 0; i < bands; i++)
                    for (int j = 0; j < ColumnCounts * LineCounts; j++)
                        BandsData[i, j] = Convert.ToInt32(BandsDataD[i, j]);

            }
            if (n != totalnum - 1)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// 字节数据转换成double数据
        /// </summary>
        /// <returns></returns>
        public bool BandHandle()
        {

            Datatemp = new double[ColumnCounts * LineCounts * bands];
            BandsDataD = new double[bands, ColumnCounts * LineCounts];
            BandsData = new int[bands, ColumnCounts * LineCounts];
            int totalnum = LineCounts * ColumnCounts * bands;
            if (DataType == 1)
            {
               
                for (int i = 0; i < totalnum; i++)
                {
                    Datatemp[i] = Convert.ToDouble(Convert.ToUInt32(bits[i]));
                }
            }
            else if (DataType == 2)
            {
                for (int i = 0; i < totalnum; i++)
                {
                    Datatemp[i] = Convert.ToDouble(BitConverter.ToInt16(bits, i * 2));
                }
            }
            else if (DataType == 4)
            {
                for (int i = 0; i < totalnum; i++)
                {
                    Datatemp[i] = Convert.ToDouble(BitConverter.ToSingle(bits, i * 4));
                }
            }
            if (Interleave.IndexOf("bsq") >= 0)
            {
                for (int i = 0; i < bands; i++)
                {
                    for (int j = 0; j < ColumnCounts * LineCounts; j++)
                    {
                        BandsDataD[i, j] = Datatemp[j + i * ColumnCounts * LineCounts];
                    }
                }
            }
            else if (Interleave.IndexOf("bil") >= 0)
            {
                for (int j = 0; j < LineCounts; j++)
                {
                    for (int i = 0; i < bands; i++)
                    {
                        for (int p = 0; p < ColumnCounts; p++)
                        {
                            BandsDataD[i, p + j * ColumnCounts] = Datatemp[p + j * ColumnCounts * bands + i * ColumnCounts];
                        }
                    }
                }
            }
            else if (Interleave.IndexOf("bip") >= 0)
            {
                for (int i = 0; i < bands; i++)
                {
                    for (int j = 0; j < LineCounts * ColumnCounts; j++)
                    {
                        BandsDataD[i, j] = Datatemp[i + j * bands];
                    }
                    
                }
            }
            else
                return false;
            return true;
        }
        public void SaveData(int ColumnCounts, int LineCounts, int bands, double[,] BandsDataD,string[] Bandsname)
        {
            this.ColumnCounts = ColumnCounts;
            this.LineCounts = LineCounts;
            this.bands = bands;
            this.BandsDataD = BandsDataD;
            this.Bandsname = Bandsname;
        }
    }
}
