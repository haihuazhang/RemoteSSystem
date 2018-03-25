using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// K-T变换方法类
    /// </summary>
    class K_Tchange
    {
        public K_Tchange(int pos)
        {
            this.pos = pos;
            this.ColumnCounts = Form1.boduan[pos].ColumnCounts;
            this.LineCounts = Form1.boduan[pos].LineCounts;
            this.bands = Form1.boduan[pos].bands;
            K_T_result = new double[3, ColumnCounts * LineCounts];
            rd = new read();
        }
        /// <summary>
        /// 静态数据位置
        /// </summary>
        private int pos;
        /// <summary>
        /// 变换系数矩阵
        /// </summary>
        public double[,] landset5 = new double[,]{{0.2909,0.2493,0.4806,0.5568,0.4438,0.1706,10.3695},
                                                  {-0.2728,-0.2174,-0.5508,0.7221,0.0733,-0.1648,-0.7310},
                                                  {0.1446,0.1761,0.3322,0.3396,-0.6210,-0.4186,-3.3828}};
        /// <summary>
        /// 结果数据
        /// </summary>
        private double[,] K_T_result;
        private int ColumnCounts, LineCounts, bands;
        private read rd;
        /// <summary>
        /// K-T计算方法
        /// </summary>
        /// <param name="BandsDataD">原double类型数组数据</param>
        /// <param name="ColumnCounts">行数</param>
        /// <param name="LineCounts">列数</param>
        /// <param name="bands">波段数</param>
        /// <returns>K_T_result</returns>
        private void ktCompute(double[,] BandsDataD)
        {
            //band为6时
            if (bands == 6)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < ColumnCounts * LineCounts; j++)
                    {
                        /// <summary>
                        /// 计算K-T值
                        /// <summary>
                        for (int k = 0; k < bands; k++)
                        {
                            K_T_result[i, j] += BandsDataD[k, j] * landset5[i, k];
                        }

                        K_T_result[i, j] += landset5[i, 6];
                    }
                }
            }
            //band为7时
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < ColumnCounts * LineCounts; j++)
                    {
                        /// <summary>
                        /// 计算K-T值
                        /// <summary>
                        for (int k = 0; k < bands - 2; k++)
                        {
                            K_T_result[i, j] += BandsDataD[k, j] * landset5[i, k];
                        }

                        K_T_result[i, j] += BandsDataD[6, j] * landset5[i, 5];
                        K_T_result[i, j] += landset5[i, 6];
                    }
                }
            }

        }
        public read GetResult()
        {
            ktCompute(Form1.boduan[pos].BandsDataD);
            rd.ColumnCounts = this.ColumnCounts;
            rd.LineCounts = this.LineCounts;
            rd.bands = 3;
            rd.BandsDataD = this.K_T_result;
            rd.Bandsname = new string[rd.bands];
            rd.BandsData = new int[rd.bands, ColumnCounts * LineCounts];
            for (int i = 0; i < rd.bands; i++)
            {
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                {
                    rd.BandsData[i, j] = (int)rd.BandsDataD[i, j];
                }
            }

            rd.DataType = 4;
            return rd;
        }

    }
}
