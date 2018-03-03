using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 梯度倒数加权平均算法
    /// </summary>
    class GradientInverseWeight
    {
        public GradientInverseWeight(int ColumnCounts, int LineCounts, int bands)
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
        /// <param name="BandsDataD"></param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <param name="bands"></param>
        /// <returns></returns>
        public void Gradient(double[,] BandsDataD)
        {
            Result = new double[bands, ColumnCounts * LineCounts];
            double[,] weight = new double[3, 3];
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
                            Result[i, j * ColumnCounts + k] = BandsDataD[i, j * ColumnCounts + k];
                        else
                        {
                            /// <summary>
                            /// 权重总和
                            /// <summary>
                            double sum = 0;
                            /// <summary>
                            /// 卷积核循环
                            /// <summary>
                            for (int p = 0; p < 3; p++)
                                for (int q = 0; q < 3; q++)
                                {
                                    /// <summary>
                                    /// 中心像素权重另行计算
                                    /// <summary>
                                    if (p == 1 && q == 1)
                                    { }
                                    else
                                    {
                                        /// <summary>
                                        /// 考虑分母为0情况
                                        /// <summary>
                                        int c;
                                        if (BandsDataD[i, (j + p - 1) * ColumnCounts + k + q - 1] != BandsDataD[i, j * ColumnCounts + k])
                                            c = 0;
                                        else
                                            c = 1;
                                        weight[p, q] = Convert.ToDouble(1) / (Math.Abs(BandsDataD[i, (j + p - 1) * ColumnCounts + k + q - 1]
                                            - BandsDataD[i, j * ColumnCounts + k]) + c);
                                        sum += weight[p, q];
                                    }
                                }
                            /// <summary>
                            /// 权重数组归一化为1/2
                            /// <summary>
                            for (int p = 0; p < 3; p++)
                                for (int q = 0; q < 3; q++)
                                    if (p != 1 || q != 1)
                                        weight[p, q] = weight[p, q] / sum / 2;
                            weight[1, 1] = 0.5;
                            for (int p = -1; p < 2; p++)
                                for (int q = -1; q < 2; q++)
                                    Result[i, j * ColumnCounts + k] += BandsDataD[i, (j + p) * ColumnCounts + k + q] * weight[p + 1, q + 1];
                        }
                    }
                }
            }
            
                        

        }
        /// <summary>
        /// 获取结果数据
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
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                    rd.BandsData[i, j] = (int)rd.BandsDataD[i, j];
            rd.DataType = 4;
            return rd;
        } 

    }
}
