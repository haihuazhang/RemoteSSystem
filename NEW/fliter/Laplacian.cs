using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 拉普拉斯算子
    /// </summary>
    class Laplacian
    {
        /// <summary>
        /// 锐化结果图像
        /// </summary>
        private double[,] Result;
        /// <summary>
        /// 梯度图像
        /// </summary>
        private double[,] Gradient;
        private int ColumnCounts, LineCounts, bands;
        private read rd;
        private read rd2;
        public Laplacian(int ColumnCounts, int LineCounts, int bands)
        {
            this.ColumnCounts = ColumnCounts;
            this.LineCounts = LineCounts;
            this.bands = bands;
            rd = new read();
            rd2 = new read();
        }
        /// <summary>
        /// 卷积核
        /// </summary>
        public int[,] Kernel = new int[3, 3] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
        public void Lapl(double[,] BandsDataD)
        {
            Result = new double[bands, ColumnCounts * LineCounts];
            Gradient = new double[bands, ColumnCounts * LineCounts];
            for (int i = 0; i < bands; i++)
            {
                for (int j = 0; j < LineCounts; j++)
                {
                    for (int k = 0; k < ColumnCounts; k++)
                    {
                        /// <summary>
                        /// 梯度图像生成
                        /// <summary>
                        /// <summary>
                        /// 图像边缘默认为0
                        /// <summary>
                        if (j == 0 || j == LineCounts - 1 || k == 0 || k == ColumnCounts - 1)
                        { }
                        /// <summary>
                        /// 卷积核运算
                        /// <summary>
                        else
                        {
                            for (int p = -1; p < 2; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    Gradient[i, j * ColumnCounts + k] += Kernel[p + 1, q + 1] * BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }
                        }
                        /// <summary>
                        /// 锐化结果图像=原图像-θ*梯度图像（θ=1）
                        /// <summary>
                        Result[i, j * ColumnCounts + k] = BandsDataD[i, j * ColumnCounts + k] - Gradient[i, j * ColumnCounts + k];
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
            {
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                {
                    rd.BandsData[i, j] = (int)rd.BandsDataD[i, j];
                }
            }

            rd.DataType = 4;
            return rd;
        }
        public read GetGrad()
        {
            rd2.ColumnCounts = this.ColumnCounts;
            rd2.LineCounts = this.LineCounts;
            rd2.bands = this.bands;
            rd2.BandsDataD = this.Gradient;
            rd2.Bandsname = new string[bands];
            rd2.BandsData = new int[bands, ColumnCounts * LineCounts];
            for (int i = 0; i < bands; i++)
            {
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                {
                    rd2.BandsData[i, j] = (int)rd2.BandsDataD[i, j];
                }
            }

            rd2.DataType = 4;
            return rd2;
        }
 
    }
}
