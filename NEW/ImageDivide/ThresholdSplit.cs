using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 阈值分割法
    /// </summary>
    class ThresholdSplit
    {
        /// <summary>
        /// 结果变量
        /// </summary>
        public int[,] result;
        /// <summary>
        /// 自适应阈值分割
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="m"></param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <returns></returns>
        public int[,] selfadapt(double[,] BandsDataD, int m, int ColumnCounts, int LineCounts)
        {
            result = new int[1, ColumnCounts * LineCounts];
            int[] pixel = new int[256];
            int T1 = 127, T2 = 0;
            int Temp1 = 0, Temp2 = 0, Temp3 = 0, Temp4 = 0;
            /// </summary>
            /// 直方图统计
            /// </summary>
            perHistogram ph = new perHistogram();
            ph.histogram2(BandsDataD, ColumnCounts * LineCounts, m, out pixel);
            
            /// </summary>
            /// 循环得到结果
            /// </summary>
            while (true)
            {
                for (int i = 0; i < T1 + 1; i++)
                {
                    Temp1 += pixel[i] * i;
                    Temp2 += pixel[i];
                }
                for (int i = T1 + 1; i < 256; i++)
                {
                    Temp3 += pixel[i] * i;
                    Temp4 += pixel[i];
                }
                T2 = (Temp1 / Temp2 + Temp3 / Temp4) / 2;
                if (T1 == T2)
                    break;
                else
                    T1 = T2;
            }
            double threw = T1 * ph.stretch / 255 + ph.min;
            for (int i = 0; i < ColumnCounts * LineCounts; i++)
            {
                if (BandsDataD[m, i] > threw)
                    result[0, i] = 255;
                else
                    result[0, i] = 0;
            }
            return result;
        }
        /// <summary>
        /// 固定阈值分割
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="m"></param>
        /// <param name="ColumnCounts"></param>
        /// <param name="LineCounts"></param>
        /// <param name="Threw"></param>
        /// <returns></returns>
        public int[,] choseThre(double[,] BandsDataD, int m, int ColumnCounts, int LineCounts, double Threw)
        {
            result = new int[1, ColumnCounts * LineCounts];
            for (int i = 0; i < ColumnCounts * LineCounts; i++)
            {
                if (BandsDataD[m, i] > Threw)
                    result[0, i] = 255;
                else
                    result[0, i] = 0;
            }
            return result;
        }
    }
}
