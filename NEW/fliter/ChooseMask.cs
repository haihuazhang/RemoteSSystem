using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 选择式掩模平滑
    /// </summary>
    class ChooseMask
    {
        public ChooseMask(int ColumnCounts,int LineCounts,int bands)
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
        /// 实现方法
        /// </summary>
        /// <param name="BandsDataD">double型原数据数组</param>
        /// <param name="bands">波段数</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <returns>MaskResult</returns>
        public void ChoseMaskPerf(double[,] BandsDataD)
        {
            /// <summary>
            /// 结果变量初始化
            /// <summary>
            Result = new double[bands, ColumnCounts * LineCounts];
            /// <summary>
            /// 掩模平均值数组
            /// <summary>
            double[] average = new double[9];
            /// <summary>
            /// 掩模方差值数组
            /// <summary>
            double[] variance = new double[9];
            for (int i = 0; i < bands; i++)
            {
                for (int j = 0; j < LineCounts; j++)
                {
                    for (int k = 0; k < ColumnCounts; k++)
                    {
                        average = new double[9];
                        variance = new double[9];
                        /// <summary>
                        /// 最外圈直接赋值
                        /// <summary>
                        if (j == 0 || j == LineCounts - 1 || k == 0 || k == ColumnCounts - 1)
                        {
                            Result[i, j * ColumnCounts + k] = BandsDataD[i, j * ColumnCounts + k];
                        }

                        /// <summary>
                        /// 向内一圈直接选择掩模5
                        /// <summary>
                        else if (((j == 1 || j == LineCounts - 2) && k >= 1 && k <= ColumnCounts - 2) ||
                            ((k == 1 || k == ColumnCounts - 2) && j >= 1 && j <= LineCounts - 2))
                        {
                            double a = 0;
                            for (int p = -1; p < 2; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    a += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }

                            a /= 9;
                            Result[i, j * ColumnCounts + k] = a;
                        }
                        /// <summary>
                        /// 其余开始最佳掩模选择
                        /// <summary>
                        else
                        {
                            /// <summary>
                            /// 掩模1
                            /// <summary>
                            for (int p = -1; p < 2; p++)
                            {
                                for (int q = -2; q < 0; q++)
                                {
                                    average[0] += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }

                            average[0] += BandsDataD[i, j * ColumnCounts + k];
                            average[0] /= 7;
                            for (int p = -1; p < 2; p++)
                            {
                                for (int q = -2; q < 0; q++)
                                {
                                    variance[0] += Math.Pow(BandsDataD[i, (j + p) * ColumnCounts + k + q]-average[0], 2);
                                }
                            }

                            variance[0] += Math.Pow(BandsDataD[i, j * ColumnCounts + k] - average[0], 2);
                            variance[0] /= 7;
                            /// <summary>
                            /// 掩模2
                            /// <summary>
                            for (int p = -2; p < 0; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    average[1] += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }

                            average[1] += BandsDataD[i, j * ColumnCounts + k];
                            average[1] /= 7;
                            for (int p = -2; p < 0; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    variance[1] += Math.Pow(BandsDataD[i, (j + p) * ColumnCounts + k + q] - average[1], 2);
                                }
                            }

                            variance[1] += Math.Pow(BandsDataD[i, j * ColumnCounts + k] - average[1], 2);
                            variance[1] /= 7;
                            /// <summary>
                            /// 掩模3
                            /// <summary>
                            for (int p = -1; p < 2; p++)
                            {
                                for (int q = 1; q < 3; q++)
                                {
                                    average[2] += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }

                            average[2] += BandsDataD[i, j * ColumnCounts + k];
                            average[2] /= 7;
                            for (int p = -1; p < 2; p++)
                            {
                                for (int q = 1; q < 3; q++)
                                {
                                    variance[2] += Math.Pow(BandsDataD[i, (j + p) * ColumnCounts + k + q] - average[2], 2);
                                }
                            }

                            variance[2] += Math.Pow(BandsDataD[i, j * ColumnCounts + k] - average[2], 2);
                            variance[2] /= 7;
                            /// <summary>
                            /// 掩模4
                            /// <summary>
                            for (int p = 1; p < 3; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    average[3] += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }

                            average[3] += BandsDataD[i, j * ColumnCounts + k];
                            average[3] /= 7;
                            for (int p = -2; p < 0; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    variance[3] += Math.Pow(BandsDataD[i, (j + p) * ColumnCounts + k + q] - average[3], 2);
                                }
                            }

                            variance[3] += Math.Pow(BandsDataD[i, j * ColumnCounts + k] - average[3], 2);
                            variance[3] /= 7;
                            /// <summary>
                            /// 掩模5
                            /// <summary>
                            for (int p = -1; p < 2; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    average[4] += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }

                            average[4] /= 9;
                            for (int p = -1; p < 2; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    variance[4] += Math.Pow(BandsDataD[i, (j + p) * ColumnCounts + k + q] - average[4], 2);
                                }
                            }

                            variance[4] /= 9;
                            /// <summary>
                            /// 掩模6
                            /// <summary>
                            for(int p=-2;p<1;p++)
                            {
                                for (int q=-2;q<1;q++)
                                {
                                    if (p == -2 && q == 0)
                                    { }
                                    else if (p == 0 && q == -2)
                                    { }
                                    else
                                    {
                                        average[5] += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                    }
                                }
                            }

                            average[5] /= 7;
                            for (int p = -2; p < 1; p++)
                            {
                                for (int q = -2; q < 1; q++)
                                {
                                    if (p == -2 && q == 0)
                                    { }
                                    else if (p == 0 && q == -2)
                                    { }
                                    else
                                    {
                                        variance[5] += Math.Pow(BandsDataD[i, (j + p) * ColumnCounts + k + q] - average[5], 2);
                                    }
                                }
                            }

                            variance[5] /= 7;
                            /// <summary>
                            /// 掩模7
                            /// <summary>
                            for (int p = -2; p < 1; p++)
                            {
                                for (int q = 0; q < 3; q++)
                                {
                                    if (p == -2 && q == 0)
                                    { }
                                    else if (p == 0 && q == 2)
                                    { }
                                    else
                                    {
                                        average[6] += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                    }
                                }
                            }

                            average[6] /= 7;
                            for (int p = -2; p < 1; p++)
                            {
                                for (int q = 0; q < 3; q++)
                                {
                                    if (p == -2 && q == 0)
                                    { }
                                    else if (p == 0 && q == 2)
                                    { }
                                    else
                                    {
                                        variance[6] += Math.Pow(BandsDataD[i, (j + p) * ColumnCounts + k + q] - average[6], 2);
                                    }
                                }
                            }

                            variance[6] /= 7;
                            /// <summary>
                            /// 掩模8
                            /// <summary>
                            for (int p = 0; p < 3; p++)
                            {
                                for (int q = -2; q < 1; q++)
                                {
                                    if (p == 2 && q == 0)
                                    { }
                                    else if (p == 0 && q == -2)
                                    { }
                                    else
                                    {
                                        average[7] += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                    }
                                }
                            }

                            average[7] /= 7;
                            for (int p = 0; p < 3; p++)
                            {
                                for (int q = -2; q < 1; q++)
                                {
                                    if (p == 2 && q == 0)
                                    { }
                                    else if (p == 0 && q == -2)
                                    { }
                                    else
                                    {
                                        variance[7] += Math.Pow(BandsDataD[i, (j + p) * ColumnCounts + k + q] - average[7], 2);
                                    }
                                }
                            }

                            variance[7] /= 7;
                            /// <summary>
                            /// 掩模9
                            /// <summary>
                            for (int p = 0; p < 3; p++)
                            {
                                for (int q = 0; q < 3; q++)
                                {
                                    if (p == 2 && q == 0)
                                    { }
                                    else if (p == 0 && q == 2)
                                    { }
                                    else
                                    {
                                        average[8] += BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                    }
                                }
                            }

                            average[8] /= 7;
                            for (int p = 0; p < 3; p++)
                            {
                                for (int q = 0; q < 3; q++)
                                {
                                    if (p == 2 && q == 0)
                                    { }
                                    else if (p == 0 && q == 2)
                                    { }
                                    else
                                    {
                                        variance[8] += Math.Pow(BandsDataD[i, (j + p) * ColumnCounts + k + q] - average[8], 2);
                                    }
                                }
                            }

                            variance[8] /= 7;
                            /// <summary>
                            /// 取方差最小的掩模的均值作为中心像素值
                            /// <summary>
                            double varmin = double.MaxValue;
                            int key=-9999;
                            for(int p=0;p<9;p++)
                            {
                                if (varmin > variance[p])
                                {
                                    varmin = variance[p];
                                    key = p;
                                }
                            }

                            Result[i, j * ColumnCounts + k] = average[key];

                        }

                    }
                }
            }

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
