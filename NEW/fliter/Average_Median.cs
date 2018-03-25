using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 均值滤波
    /// </summary>
    public class Average_Median
    {
                /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <param name="bands"></param>
        public Average_Median(int ColumnCounts,int LineCounts,int bands)
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
        /// 结果变量
        /// </summary>
        private double[,] Result;
        /// <summary>
        /// 均值滤波实现方法
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <param name="bands"></param>
        /// <returns></returns>
        public double[,] AverSmoothing(double[,] BandsDataD)
        {
            Result = new double[bands, ColumnCounts * LineCounts];
            //double[,] weight = new double[3, 3];
            for (int i = 0; i < bands; i++)
            {
                /// <summary>
                /// 图像行列
                /// <summary>
                for (int j = 0; j < LineCounts; j++)
                {
                    for (int k = 0; k < ColumnCounts; k++)
                    {
                        /// <summary>
                        /// 考虑边缘情况
                        /// <summary> 
                        if (j == 0 || j == LineCounts - 1 || k == 0 || k == ColumnCounts - 1)
                        {
                            Result[i, j * ColumnCounts + k] = BandsDataD[i, j * ColumnCounts + k];
                        }
                        else
                        {

                            /// <summary>
                            /// 8-邻域像素值总和
                            /// <summary>
                            double sum = 0;
                            /// <summary>
                            /// 卷积核循环
                            /// <summary>
                            for (int p = -1; p <= 1; p++)
                            {
                                for (int q = -1; q <= 1; q++)
                                {
                                    sum += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }
                            Result[i, j * ColumnCounts + k] = sum / 9;
                        }
                    }

                }
            }
            return Result;
        }
        /// <summary>
        /// 中值滤波
        /// </summary>
        /// <param name="BandsDataD">double型原数据</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="bands">波段数</param>
        /// <returns>Result</returns>
        public double[,] MedianSmoothing(double[,] BandsDataD)
        {

            Result = new double[bands, ColumnCounts * LineCounts];
            /// <summary>
            /// 波段
            /// <summary>
            for (int i = 0; i < bands; i++)
            {
                /// <summary>
                /// 图像行列
                /// <summary>
                for (int j = 0; j < ColumnCounts; j++)
                {
                    for (int k = 0; k < LineCounts; k++)
                    {
                        /// <summary>
                        /// 考虑边界问题
                        /// <summary>
                        if (j == 0 || j == LineCounts - 1 || k == 0 || k == ColumnCounts - 1)
                        {
                            Result[i, j * ColumnCounts + k] = BandsDataD[i, j * ColumnCounts + k];
                        }

                        /// <summary>
                        /// 利用List自带快速排序对卷积核内数据进行排序，得到中值
                        /// <summary>
                        else
                        {
                            List<double> getMedian = new List<double>();
                            /// <summary>
                            /// 压栈
                            /// <summary>
                            for (int p = -1; p < 2; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    getMedian.Add(BandsDataD[i, (j + p) * ColumnCounts + k + q]);
                                }
                            }

                            /// <summary>
                            /// 快速排序
                            /// <summary>
                            getMedian.Sort();
                            /// <summary>
                            /// 结果赋值
                            /// <summary>
                            Result[i, j * ColumnCounts + k] = getMedian[4];
                        }
                    }
                }
            }

            return Result;
        }
        /// <summary>
        /// 获取结果数据块
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

 

