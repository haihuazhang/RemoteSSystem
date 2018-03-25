using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 高斯低通滤波
    /// </summary>
    class GassLowPass
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <param name="bands"></param>
        public GassLowPass(int ColumnCounts,int LineCounts,int bands)
        {
            this.ColumnCounts = ColumnCounts;
            this.LineCounts = LineCounts;
            this.bands = bands;
            Result = new double[bands, ColumnCounts * LineCounts];
            rd = new read();
        }
        /// <summary>
        /// 结果数据
        /// </summary>
        private read rd;
        /// <summary>
        /// 必要参数
        /// </summary>
        private int ColumnCounts, LineCounts, bands;
        /// <summary>
        /// 结果数组
        /// </summary>
        private double[,] Result;
        /// <summary>
        /// 得到高斯低通滤波处理结果
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="border"></param>
        /// <param name="variance"></param>
        public void GetGassValue(double[,] BandsDataD, int border, int variance)
        {
            double[,] K = GassFliter(border, variance);
            for (int i = 0; i < bands; i++)
            {
                for (int j = 0; j < LineCounts; j++)
                {
                    for (int k = 0; k < ColumnCounts; k++)
                    {
                        if (j < border / 2 || j > (LineCounts - 1 - border / 2) || k < border / 2 || k > (ColumnCounts - 1 - border / 2))
                        {
                            Result[i, j * ColumnCounts + k] = BandsDataD[i, j * ColumnCounts + k];
                        }
                        else
                        {
                            for (int p = -border / 2; p < border / 2; p++)
                            {
                                for (int q = -border / 2; q < border / 2; q++)
                                {
                                    Result[i, j * ColumnCounts + k] += K[p + border / 2, q + border / 2] * BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 计算高斯低通卷积核
        /// </summary>
        /// <param name="border"></param>
        /// <param name="variance"></param>
        /// <returns></returns>
        private double[,] GassFliter(int border, int variance)
        {
            double[,] fliterT = new double[border, border];
            for (int i = -border / 2; i < border / 2; i++)
            {
                for (int j = -border / 2; j < border / 2; j++)
                {
                    fliterT[i + border / 2, j + border / 2] = Math.Pow(Math.E, -(i * i + j * j) / ((double)(2 * variance * variance)));
                }
            }
            double sum =0.0;
            for (int i = 0; i < border; i++)
            {
                for (int j = 0; j < border; j++)
                {
                    sum += fliterT[i, j];
                }
            }

            for (int i = 0; i < border; i++)
            {
                for (int j = 0; j < border; j++)
                {
                    fliterT[i, j] /= sum;
                }
            }

            return fliterT;
        }
        
        /// <summary>
        /// 得到整个数据，缺少波段名和文件名
        /// </summary>
        /// <returns></returns>
        public read GetResult()
        {
            rd.ColumnCounts = this.ColumnCounts;
            rd.LineCounts = this.LineCounts;
            rd.bands = this.bands;
            rd.BandsDataD = this.Result;
            rd.Bandsname = new string[bands];
            rd.BandsData = new int[bands, ColumnCounts * LineCounts];
            for (int i = 0; i < bands; i++)
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
