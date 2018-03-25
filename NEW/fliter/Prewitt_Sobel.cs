using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// Prewitt和Sobel梯度算法
    /// </summary>
    class Prewitt_Sobel
    {
        /// <summary>
        /// 结果变量（梯度+原始）
        /// </summary>
        public double[,] Result;
        /// <summary>
        /// 梯度图像
        /// </summary>
        public double[,] Gradient;
        public double[,] Gradient2;
        /// <summary>
        /// Prewitt卷积核
        /// </summary>
        public int[,] Pre1 = new int[3, 3] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
        public int[,] Pre2 = new int[3, 3] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
        /// <summary>
        /// Sobel卷积核
        /// </summary>
        public int[,] Sob1 = new int[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
        public int[,] Sob2 = new int[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
        //private int flag;
        private int ColumnCounts, LineCounts, bands;
        private read rd, rd2;
        public Prewitt_Sobel(int ColumnCounts, int LineCounts, int bands)
        {
            //this.flag = flag;
            this.ColumnCounts = ColumnCounts;
            this.LineCounts = LineCounts;
            this.bands = bands;
            rd = new read();
            rd2 = new read();
        }

        /// <summary>
        /// 水平向Pre梯度
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="bands"></param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <returns></returns>
        public void PrePer1(double[,] BandsDataD)
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
                                    Gradient[i, j * ColumnCounts + k] += Pre1[p + 1, q + 1] * BandsDataD[i, (j + p) * ColumnCounts + k + q];
                                }
                            }
                        }
                        /// <summary>
                        /// 锐化结果图像=原图像*梯度图像（θ=1）
                        /// <summary>
                        Result[i, j * ColumnCounts + k] = BandsDataD[i, j * ColumnCounts + k] - Gradient[i, j * ColumnCounts + k];
                    }
                }
            }
                     
        }
        /// <summary>
        /// 垂直向Pre梯度
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="bands"></param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <returns></returns>
        public void PrePer2(double[,] BandsDataD)
        {
            Result = new double[bands, ColumnCounts * LineCounts];
            Gradient2 = new double[bands, ColumnCounts * LineCounts];
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
                                    Gradient2[i, j * ColumnCounts + k] += Pre2[p + 1, q + 1] * BandsDataD[i, (j + p) * ColumnCounts + k + q];
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
        /// 水平向Sobel梯度
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="bands"></param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <returns></returns>
        public void SobPer1(double[,] BandsDataD)
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
                                    Gradient[i, j * ColumnCounts + k] += Sob1[p + 1, q + 1] * BandsDataD[i, (j + p) * ColumnCounts + k + q];
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
        /// 垂直向Sobel梯度
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="bands"></param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <returns></returns>
       public void SobPer2(double[,] BandsDataD)
        {
            Result = new double[bands, ColumnCounts * LineCounts];
            Gradient2 = new double[bands, ColumnCounts * LineCounts];
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
                                    Gradient2[i, j * ColumnCounts + k] += Sob2[p + 1, q + 1] * BandsDataD[i, (j + p) * ColumnCounts + k + q];
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
        /// 获取梯度1
        /// </summary>
        /// <returns></returns>
       public read GetGrad1()
       {
           
           rd.ColumnCounts = this.ColumnCounts;
           rd.LineCounts = this.LineCounts;
           rd.bands = this.bands;
           rd.BandsDataD = this.Gradient;

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
        /// <summary>
        /// 获取梯度2
        /// </summary>
        /// <returns></returns>
       public read GetGrad2()
       {
           rd2.ColumnCounts = this.ColumnCounts;
           rd2.LineCounts = this.LineCounts;
           rd2.bands = this.bands;
           rd2.BandsDataD = this.Gradient2;
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
