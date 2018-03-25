using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// RGB转HSI，方法：标准模型法
    /// </summary>
    class RGBToHSI
    {
        /// <summary>
        /// 结果数据
        /// </summary>
        private double[,] HSIColor;
        private read rd;
        private double[,] BandsDataD;
        public RGBToHSI(int iR,int iG,int iB,int Rband,int Gband,int Bband,read rd)
        {
            this.rd = rd;
            /// <summary>
            /// rd各项数据初始化
            /// <summary>
            this.rd.bands = 3;
            this.rd.ColumnCounts = Form1.boduan[iR].ColumnCounts;
            this.rd.LineCounts = Form1.boduan[iR].LineCounts;
            this.rd.DataType = 4;
            this.rd.BandsDataD = new double[3, rd.ColumnCounts * rd.LineCounts];
            this.rd.BandsData = new int[3, rd.ColumnCounts * rd.LineCounts];
            /// <summary>
            ///任意图像任意波段合成（数据传递）
            /// <summary>
            for (int j = 0; j < rd.ColumnCounts * rd.LineCounts; j++)
            {
                this.rd.BandsDataD[0, j] = Form1.boduan[iR].BandsDataD[Rband, j];
            }

            for (int j = 0; j < rd.ColumnCounts * rd.LineCounts; j++)
            {
                this.rd.BandsDataD[1, j] = Form1.boduan[iG].BandsDataD[Gband, j];
            }

            for (int j = 0; j < rd.ColumnCounts * rd.LineCounts; j++)
            {
                this.rd.BandsDataD[2, j] = Form1.boduan[iB].BandsDataD[Bband, j];
            }
            //传入原始数据
            this.BandsDataD = this.rd.BandsDataD;
        }

        /// <summary>
        /// 转换方法(弃用)
        /// </summary>
        /// <param name="BandsDataD">RGB数据</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="bands">波段数</param>
        /// <returns>HSIColor|HSI格式数据</returns>
        private double[,] ConvertHSI()
        {
            /// </summary>
            /// 数据初始化
            /// </summary>
            HSIColor = new double[3, rd.ColumnCounts * rd.LineCounts];

            double R, G, B;
            for (int i = 0; i < rd.ColumnCounts * rd.LineCounts; i++)
            {
                /// </summary>
                /// R、G、B归一化
                /// </summary>
                R = BandsDataD[0, i] / 255;
                G = BandsDataD[1, i] / 255;
                B = BandsDataD[2, i] / 255;
                
                /// </summary>
                /// 开始颜色转换
                /// </summary>
               
                    /// </summary>
                    /// 取最大值
                    /// </summary>
                    double Max = Math.Max(R, G);
                    Max = Math.Max(Max, B);
                    /// </summary>
                    /// 取最小值
                    /// </summary>
                    double Min = Math.Min(R, G);
                    Min = Math.Min(Min, B);
                    /// </summary>
                    /// S分量计算
                    /// </summary>
                    HSIColor[1, i] = Max - Min;
                    /// </summary>
                    /// I分量计算
                    /// </summary>
                    HSIColor[2, i] = (Max + Min) / 2;
                    /// </summary>
                    /// H分量计算
                    /// </summary>
                    /// </summary>
                    /// 特殊情况处理（R=G=B）
                    /// </summary>
                    if (R == G && R == B)
                {
                    HSIColor[0, i] = 0;
                }
                else
                    {
                        if (Max == R)
                    {
                        HSIColor[0, i] = Math.PI / 3 * (G - B) / (Max - Min);
                    }
                    else if (Max == G)
                    {
                        HSIColor[0, i] = Math.PI / 3 * (B - R) / (Max - Min) + 2.0 / 3.0 * Math.PI;
                    }
                    else
                    {
                        HSIColor[0, i] = Math.PI / 3 * (R - G) / (Max - Min) + 4.0 / 3.0 * Math.PI;
                    }

                    if (HSIColor[0, i] < 0)
                    {
                        HSIColor[0, i] = HSIColor[0, i] + 2 * Math.PI;
                    }
                }
                    /// </summary>
                    /// 弧度转角度
                    /// </summary>
                    HSIColor[0, i] = HSIColor[0, i] * 180 / Math.PI;
            }
            /// </summary>
            /// return
            /// </summary>
            return HSIColor;
    
        }
        /// <summary>
        /// 转换方法(version 2.0)
        /// </summary>
        /// <param name="BandsDataD">RGB数据</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="bands">波段数</param>
        /// <returns>HSIColor|HSI格式数据</returns>
        public double[,] ConvertHSI2()
        {
            HSIColor = new double[3, rd.ColumnCounts * rd.LineCounts];
            double R, G, B;
            for (int i = 0; i < rd.ColumnCounts * rd.LineCounts; i++)
            {
                /// </summary>
                /// R、G、B归一化
                /// </summary>
                R = BandsDataD[0, i] / 255;
                G = BandsDataD[1, i] / 255;
                B = BandsDataD[2, i] / 255;
                double Rr = BandsDataD[0, i];
                double Gg = BandsDataD[1, i];
                double Bb = BandsDataD[2, i];
                //double xita = Math.Acos(1);
                HSIColor[2, i] = (R + B + G) / 3;
                /// </summary>
                /// 取最小值
                /// </summary>
                double Min = Math.Min(R, G);
                Min = Math.Min(Min, B);
                HSIColor[1, i] = 1 - 3 * Min / (R + G + B);
                if (R == G && R == B)
                {
                    HSIColor[0, i] = 0;
                }
                else
                {

                    double θ = Math.Acos((2 * Rr - Gg - Bb) / 2 / Math.Sqrt(Math.Pow(Rr - Gg, 2) + (Rr - Bb) * (Gg - Bb)));
                    θ *= (180 / Math.PI);
                    if (B <= G)
                    {
                        HSIColor[0, i] = θ;
                    }
                    else
                    {
                        HSIColor[0, i] = 360 - θ;
                    }
                }

            }
            return HSIColor;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public read GetResult()
        {
            rd.BandsDataD = ConvertHSI2();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < rd.ColumnCounts * rd.LineCounts; j++)
                {
                    rd.BandsData[i, j] = (int)rd.BandsDataD[i, j];
                }
            }

            return rd;
        }
    }
}
