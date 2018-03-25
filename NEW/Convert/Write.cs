using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RemoteSystem
{
    class Write
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="InterLeave"></param>
        /// <param name="OrginHDR"></param>
        public Write(read rd,string InterLeave,string OrginHDR)
        {
            this.rd = rd;
            this.InterLeave = InterLeave;
            this.OrginHDR = OrginHDR;
        }
        read rd;
        string InterLeave;
        string OrginHDR;
        //public bool isSuccessed = false;
        /// <summary>
        /// 写入数据文件
        /// </summary>
        /// <param name="DataPath"></param>
        public void WriteData(string DataPath)
        {
            FileStream fs = new FileStream(DataPath, FileMode.Create);
            if (InterLeave == "bil")
            {
                if (rd.DataType == 1)
                {
                    for(int i=0;i<rd.LineCounts;i++)
                    {
                        for (int j=0;j<rd.bands;j++)
                        {
                            for (int k = 0; k < rd.ColumnCounts; k++)
                            {
                                int temp = (int)rd.BandsDataD[j, k + i * rd.ColumnCounts];
                                fs.WriteByte((byte)temp);
                            }
                        }
                    }
                }
                else if (rd.DataType == 2)
                {
                    for (int i = 0; i < rd.LineCounts; i++)
                    {
                        for (int j = 0; j < rd.bands; j++)
                        {
                            for (int k = 0; k < rd.ColumnCounts; k++)
                            {

                                fs.Write(BitConverter.GetBytes(Convert.ToInt16(rd.BandsDataD[j, k + i * rd.ColumnCounts])), 0, 2);
                            }
                        }
                    }
                }
                else if (rd.DataType == 4)
                {
                    for (int i = 0; i < rd.LineCounts; i++)
                    {
                        for (int j = 0; j < rd.bands; j++)
                        {
                            for (int k = 0; k < rd.ColumnCounts; k++)
                            {

                                fs.Write(BitConverter.GetBytes(Convert.ToSingle(rd.BandsDataD[j, k +i * rd.ColumnCounts])), 0, 4);
                            }
                        }
                    }
                }

            }
            else if (InterLeave == "bip")
            {
                if (rd.DataType == 1)
                {
                    for (int i = 0; i < rd.ColumnCounts * rd.LineCounts;i++ )
                    {
                        for (int j = 0; j < rd.bands; j++)
                        {
                            int temp = (int)rd.BandsDataD[j, i];
                            fs.WriteByte((byte)temp);
                        }
                    }
                }
                else if(rd.DataType==2)
                {
                    for (int i = 0; i < rd.ColumnCounts * rd.LineCounts; i++)
                    {
                        for (int j = 0; j < rd.bands; j++)
                        {
                            fs.Write(BitConverter.GetBytes(Convert.ToInt16(rd.BandsDataD[j, i])), 0, 2);
                        }
                    }
                }
                else if(rd.DataType==4)
                {
                    for (int i = 0; i < rd.ColumnCounts * rd.LineCounts; i++)
                    {
                        for (int j = 0; j < rd.bands; j++)
                        {
                            fs.Write(BitConverter.GetBytes(Convert.ToSingle(rd.BandsDataD[j, i])), 0, 4);
                        }
                    }
                }
            }
            else if (InterLeave == "bsq")
            {
                if (rd.DataType == 1)
                {
                    for (int j = 0; j < rd.bands; j++)
                    {
                        for (int i = 0; i < rd.ColumnCounts * rd.LineCounts; i++)
                        {
                            int temp = (int)rd.BandsDataD[j, i];
                            fs.WriteByte((byte)temp);
                        }
                    }
                }
                else if (rd.DataType == 2)
                {
                    for(int i=0;i<rd.bands;i++)
                    {
                        for (int j = 0; j < rd.ColumnCounts * rd.LineCounts; j++)
                        {
                            fs.Write(BitConverter.GetBytes(Convert.ToInt16(rd.BandsDataD[i, j])), 0, 2);
                        }
                    }
                }
                else if (rd.DataType == 4)
                {
                    for (int i = 0; i < rd.bands; i++)
                    {
                        for (int j = 0; j < rd.ColumnCounts * rd.LineCounts; j++)
                        {
                            fs.Write(BitConverter.GetBytes(Convert.ToSingle(rd.BandsDataD[i, j])), 0, 4);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 写入头文件信息
        /// </summary>
        /// <param name="DataPath"></param>
        public void WriteHDR(string DataPath)
        {
            string hdrPath = DataPath + ".hdr";
            StreamReader sr = new StreamReader(OrginHDR);
            StreamWriter sw = new StreamWriter(hdrPath);
            string Content ="";
            while ((Content = sr.ReadLine()) != null)
            {
                if (Content.IndexOf("interleave") > -1 )
                {
                    Content = "interleave = "+InterLeave;
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
