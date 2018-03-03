using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    class imagestretch
    {
        public imagestretch(int ColumnCounts, int LineCounts, int bands)
        {
            this.ColumnCounts = ColumnCounts;
            this.LineCounts = LineCounts;
            this.bands = bands;
            this.stretResult = new double[bands, ColumnCounts * LineCounts];       
        }
        private int ColumnCounts, LineCounts, bands;
        private double[,] stretResult;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BandsData">传入数据</param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <returns>0-255全波段数据</returns>
        public void LinearShow( int[,] showData ,double[,] BandsDataD)
        {

            for (int i = 0; i < bands; i++)
            {
                double fmax = -99999; double fmin = 99999;
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                {
                    if (fmax < BandsDataD[i, j])
                        fmax = BandsDataD[i, j];
                    if (fmin > BandsDataD[i, j])
                        fmin = BandsDataD[i, j];
                }

                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                {
                    //对分母为0情况特殊处理
                    if (fmax == fmin)
                        showData[i, j] = (int)Math.Abs(fmin);
                    else
                        showData[i, j] = Convert.ToInt32((Convert.ToDouble(255) * (BandsDataD[i, j] - fmin) / (fmax - fmin)));
                }
            }


        }
        /// <summary>
        /// 2%Linear 
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <param name="m">波段号</param>
        public int[,] TwoPercentStretch(double[,] BandsDataD)
        {
            int[,] bandstemp = new int[bands, ColumnCounts * LineCounts];
            int[] pixel = new int[256];
            double c = 1; double d = 255;
            for (int p = 0; p < bands; p++)
            {
                int a = 0; int b = 0;
                int count2 = 0;
                int count98 = 0;
                perHistogram ph = new perHistogram();
                ph.histogram2(BandsDataD, ColumnCounts * LineCounts, p, out pixel);

                for (int i = 0; i < 256; i++)
                {
                    for (int j = 1; j <= pixel[i]; j++)
                    {
                        count2++;
                        count98++;
                        if (count2 == Convert.ToInt32(0.02 * ColumnCounts * LineCounts))
                        {
                            a = i;
                        }
                        if (count98 == Convert.ToInt32(0.98 * ColumnCounts * LineCounts))
                        {
                            b = i;
                        }
                    }
                }
                //特殊情况处理
                if (a == b)
                {
                    if (a <= 0)
                    {
                        for (int i = 0; i < ColumnCounts * LineCounts; i++)
                            bandstemp[p, i] = 0;
                    }
                    else if (a >= 255)
                    {
                        for (int i = 0; i < ColumnCounts * LineCounts; i++)
                            bandstemp[p, i] = 255;
                    }
                    else
                    {

                    }
                }
                //空间范围内原灰度级不连续、值差别分布情况
                else
                {

                    for (int i = 0; i < ColumnCounts * LineCounts; i++)
                    {
                        int Comp =(int) Math.Ceiling((BandsDataD[p, i] - ph.min) * 255 / ph.stretch);
                        if (Comp< a)
                        {
                            bandstemp[p, i] = (int)c;
                        }
                        else if (Comp > b)
                        {
                            bandstemp[p, i] = (int)d;
                        }
                        else if (Comp >= a && Comp <= b)
                        {
                            double a1 = (a-1) * ph.stretch / 255 + ph.min;
                            double b1 = b * ph.stretch / 255 + ph.min;
                            bandstemp[p, i] = (int)((d - c) / (b1 - a1) * (BandsDataD[p, i] - a1) + c);

                        }
                    }
                }
            }
            return bandstemp;
        }


        /// <summary>
        /// 任意范围线性拉伸
        /// </summary>
        /// <param name="BandsDataD">0-255显示数据</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="a">原最小灰度级</param>
        /// <param name="b">原最大灰度级</param>
        /// <param name="c">目标最小灰度级</param>
        /// <param name="d">目标最大灰度级</param>
        /// <param name="m">波段号</param>
        public void GlobalLinear(double[,] BandsDataD,int[,] bandstemp,  double a, double b, int m)
        {
            //特殊情况处理

            if (a == b)
            {
                if (a < 0)
                {
                    for (int i = 0; i < ColumnCounts * LineCounts; i++)
                        bandstemp[m, i] = 0;
                }
                else if (a > 255)
                {
                    for (int i = 0; i < ColumnCounts * LineCounts; i++)
                        bandstemp[m, i] = 255;
                }
                else
                {

                }
            }
            else
            {
                for (int i = 0; i < LineCounts*ColumnCounts; i++)
                {
                   
                        if (BandsDataD[m,  i] >= a && BandsDataD[m,  i ] <= b)
                        {
                            bandstemp[m,  i ] =(int)((BandsDataD[m,  i ]- a)*255 / (b - a));
                        }
                        else if (BandsDataD[m, i  ] < a)
                            bandstemp[m,   i  ] = 1;
                        else if (BandsDataD[m,   i  ] > b)
                            bandstemp[m,   i  ] = 255;
                    
                }
            }

        }

        /// <summary>
        /// 直方图匹配（规定化）
        /// </summary>
        /// <param name="BandDataD1">待匹配图像</param>
        /// <param name="BandDataD2">目标图像</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="bands">波段数</param>
        /// <param name="Reflect">灰度级映射</param>
        public void HistogramMatch(double[,] BandDataD1, double[,] BandDataD2,int Tarbands)
        {
            double[,] Reflect = new double[bands, 256];
            //目标图像波段序号
            int m;
            /// </summary>
            /// 各波段...
            /// </summary>
            for(int k=0;k<bands;k++)
            {
                if (Tarbands == 1)
                    m = 0;
                else
                    m = k;
                
                int[] orgin = new int[256]; int[] target = new int[256]; int[] pixel1 = new int[256]; int[] pixel2 = new int[256];
                int[,] srcMin = new int[256, 256]; int min; int Y=0;
                /// </summary>
                /// 计算原图像累计直方图
                /// </summary>
                perHistogram ph1 = new perHistogram();
                ph1.histogram2(BandDataD1, ColumnCounts * LineCounts, k,out pixel1);
                for (int i = 1; i < 256; i++)//计算原图像累计直方图
                    for (int j = 1; j <= i; j++)
                        if(pixel1[i]>0)
                        orgin[i] += pixel1[j];

                /// </summary>
                /// 计算目标图像累计直方图
                /// </summary>
                perHistogram ph2 =new perHistogram();
                ph2.histogram2(BandDataD2, ColumnCounts * LineCounts, m, out pixel2);
                for (int i = 1; i < 256; i++)//计算目标图像累计直方图
                    for (int j = 1; j <= i; j++)
                        if(pixel2[i]>0)
                        target[i] += pixel2[j];

                /// </summary>
                /// 两直方图直方图互减，得到[255,255]二维差值
                /// </summary>
                for (int i = 1; i < 256; i++)
                {
                    for (int j = 1; j < 256; j++)
                    {
                        srcMin[i, j] = Math.Abs(orgin[i] - target[j]);
                    }
                }
                /// </summary>
                /// 待匹配图像每灰度级对应目标图像差值最小的灰度级
                /// </summary>
                for (int i = 1; i < 256; i++)
                {
                    min = 9999;
                    for (int j = 1; j < 256; j++)
                    {
                        if (orgin[i] > 0 && target[j] > 0)
                        {
                            if (min > srcMin[i, j])
                            {
                                min = srcMin[i, j];
                                Y = j;
                            }
                        }
                    }
                    /// </summary>
                    /// 得到最接近的灰度级映射
                    /// </summary>
                    Reflect[k, i] = (double)Y * ph2.stretch / (double)255 + ph2.min;    
                    
                }
                /// </summary>
                /// 开始映射
                /// </summary>
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                {
                    int T = (int)Math.Ceiling((BandDataD1[k, j] - ph1.min) / (ph1.max - ph1.min) * 255);
                    stretResult[k, j] = (Reflect[k, T] - ph2.min) / (ph2.max - ph2.min) * (ph1.max - ph1.min) + ph1.min;

                }
            }
        }
        /// <summary>
        /// 直方图均衡化
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="m"></param>
        public void Equalization(double[,] BandsDataD)
        {
            int N;//像素点个数
            int L;
            int[] pixel ;//像素直方图
            int[] overlay ; //累积直方图
            double[] overlayD;//累积频率数组
            double[] newpixel;//灰度映射数组
            for (int m = 0; m < bands; m++)
            {
                /// </summary>
                /// 统计图像像素点个数
                /// </summary>
                L = 0;
                N = 0;
                pixel = new int[256];
                overlay = new int[256];
                overlayD = new double[256];
                newpixel = new double[256];
                for (int i = 0; i < ColumnCounts * LineCounts; i++)
                    if (BandsDataD[m, i] !=null)
                        N++;

                perHistogram ph = new perHistogram();
                /// </summary>
                ///计算像素直方图
                /// </summary>
                ph.histogram2(BandsDataD, ColumnCounts * LineCounts, m, out pixel);
                /// </summary>
                ///计算累计直方图overlay[,]
                /// </summary>
                for (int i = 1; i < 256; i++)
                    for (int j = 1; j <= i; j++)
                        if (pixel[i] > 0)
                            overlay[i] += pixel[j];

                for (int i = 0; i < 256; i++)
                    if (pixel[i] > 0)
                        L++;


                //原灰度级像素累积频率和理论像素累积频率差值数组。
                double[,] srcMin = new double[256, 256];
                double min; int Y = 0;

                /// </summary>
                /// 新像素值
                /// </summary>
                for (int i = 1; i < 256; i++)
                {
                    overlayD[i] = Convert.ToDouble(overlay[i]) / N;
                    for (int j = 1; j < L; j++)
                    {
                        srcMin[i, j] = Math.Abs(overlayD[i] - (double)j / L);
                    }
                }
                for (int i = 1; i < 256; i++)
                {
                    min = 9999;
                    for (int j = 1; j < L; j++)
                    {
                        if (min > srcMin[i, j])
                        {
                            min = srcMin[i, j];
                            Y = j;//得到灰度映射
                        }
                    }
                    /// </summary>
                    /// 得到最接近的灰度级映射
                    /// </summary>
                    newpixel[i] = Y;
                }

                for (int i = 1; i < 256; i++)
                {
                    newpixel[i] = (newpixel[i] - 1) / (L-1) * ph.stretch + ph.min;
                }
                /// </summary>
                /// 替换像素
                /// </summary>
                for (int i = 0; i < ColumnCounts * LineCounts; i++)
                {
                    int k = (int)Math.Ceiling((BandsDataD[m, i] - ph.min) / ph.stretch * (double)255);
                    stretResult[m, i] = newpixel[k];
                }
            }

        }
        /// <summary>
        /// 输出结果函数
        /// </summary>
        /// <returns></returns>
        public double[,] getresult()
        {
            return stretResult;
        }

        internal void LinearShow(int[,] showdata, int p, int ColumnCounts, int LineCounts, int bands)
        {
            throw new NotImplementedException();
        }
    }

}
