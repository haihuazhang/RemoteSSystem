using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RemoteSystem
{
    /// <summary>
    /// 保存文件类
    /// </summary>
    class SaveFile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="dataPATH"></param>
        /// <param name="DataType"></param>
        public SaveFile(read rd,string dataPATH,int DataType)
        {
            this.DataType=DataType;
            this.rd = rd;
            this.dataPATH=dataPATH;
            this.HdrPATH=dataPATH+".hdr";
        }
        private read rd;
        private string dataPATH;
        private string HdrPATH;
        private int DataType;
        /// <summary>
        /// 存储数据文件
        /// </summary>
        /// <returns></returns>
        public bool SaveDataF()
        {
            FileStream fs = new FileStream(dataPATH,FileMode.Create);
            if (DataType == 1)
            {
                for (int i = 0; i < rd.bands; i++)
                    for (int j = 0; j < rd.ColumnCounts * rd.LineCounts; j++)
                        fs.WriteByte(Convert.ToByte(rd.BandsDataD[i, j]));
            }
            else if (DataType == 2)
                for (int i = 0; i < rd.bands; i++)
                    for (int j = 0; j < rd.ColumnCounts * rd.LineCounts; j++)
                        fs.Write(BitConverter.GetBytes(Convert.ToInt16(rd.BandsDataD[i, j])), 0, 2);
            else if(DataType==4)
                for (int i = 0; i < rd.bands; i++)
                    for (int j = 0; j < rd.ColumnCounts * rd.LineCounts; j++)
                        fs.Write(BitConverter.GetBytes((float)(rd.BandsDataD[i, j])), 0, 4);
            fs.Flush();
            fs.Close();
            return true;
        }
        /// <summary>
        /// 存储HDR文件（头文件）
        /// </summary>
        public void Savehdr()
        {
            FileStream fs = new FileStream(HdrPATH, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("ENVI");
            sw.WriteLine("description = {");
            sw.WriteLine("  Create New File Result [Thu Nov 10 14:38:50 2005]}");
            sw.WriteLine("samples = {0}", rd.ColumnCounts);
            sw.WriteLine("lines = {0}", rd.LineCounts);
            sw.WriteLine("bands   = {0}", rd.bands);
            sw.WriteLine("header offset = 0");
            sw.WriteLine("file type = ENVI Standard");
            sw.WriteLine("data type = {0}", DataType);
            sw.WriteLine("interleave = bsq");
            sw.WriteLine("sensor type = Landsat TM");
            sw.WriteLine("byte order = 0");
            sw.WriteLine("wavelength units = Unknown");
            sw.WriteLine("pixel size = {3.00000000e+001, 3.00000000e+001, units=Meters}");
           

            if (rd.Bandsname[0] == "band" + (0 + 1)) { }
            else
            {
                sw.WriteLine("band names = {");
                for (int i = 0; i < rd.bands - 1; i++)
                {
                    sw.Write(" {0},", rd.Bandsname[i]);
                }
                sw.Write(" {0}", rd.Bandsname[rd.bands-1]);
                sw.Write("}");
            }
            
            
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
