using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 基本波段运算
    /// </summary>
    class BandMath
    {
        /// <summary>
        /// 波段运算结果数据
        /// </summary>
        public double[] bandplusresult;
        public double[] bandminusresult;
        public double[] bandMULTresult;
        public double[] banddivideresult;
        /// <summary>
        /// 波段相加
        /// </summary>
        /// <param name="BandsDataD1">加成员</param>
        /// <param name="BandsDataD2">加成员</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="m">波段号1</param>
        /// <param name="n">波段号2</param>
        /// <param name="expression"></param>
        /// <returns>波段相加结果</returns>
        public double[] bandPlus(double[] BandsDataD1,double[] BandsDataD2)
        {
            bandplusresult = new double[BandsDataD1.GetLength(0)];
            for (int i = 0; i < BandsDataD1.GetLength(0); i++)
            {
                bandplusresult[i] = BandsDataD1[ i] + BandsDataD2[ i];
            }
            return bandplusresult;
        }
        /// <summary>
        /// 波段相减
        /// </summary>
        /// <param name="BandsDataD1">减成员</param>
        /// <param name="BandsDataD2">减成员</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="m">波段号1</param>
        /// <param name="n">波段号2</param>
        /// <returns>波段相减结果</returns>
        public double[] bandMinus(double[] BandsDataD1, double[] BandsDataD2)
        {
            bandminusresult = new double[BandsDataD1.GetLength(0)];
            for (int i = 0; i < BandsDataD1.GetLength(0); i++)
            {
                bandminusresult[i] = BandsDataD1[i] - BandsDataD2[ i];
            }
            return bandminusresult;
        }
        /// <summary>
        /// 波段相乘
        /// </summary>
        /// <param name="BandsDataD1">乘成员</param>
        /// <param name="BandsDataD2">乘成员</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="m">波段号1</param>
        /// <param name="n">波段号2</param>
        /// <returns>波段相乘结果</returns>
        public double[] bandMULT(double[] BandsDataD1, double[] BandsDataD2)
        {
            bandMULTresult = new double[BandsDataD1.GetLength(0)];
            for (int i = 0; i < BandsDataD1.GetLength(0); i++)
            {
                bandMULTresult[i] = BandsDataD1[ i] * BandsDataD2[ i];
            }
            return bandMULTresult;
        }
        /// <summary>
        /// 波段相除
        /// </summary>
        /// <param name="BandsDataD1">除成员</param>
        /// <param name="BandsDataD2">除成员</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="m">波段号1</param>
        /// <param name="n">波段号2</param>
        /// <returns>波段相除结果</returns>
        public double[] bandDivide(double[] BandsDataD1, double[] BandsDataD2)
        {
            banddivideresult = new double[BandsDataD1.GetLength(0)];
            for (int i = 0; i < BandsDataD1.GetLength(0); i++)
            {
                if (BandsDataD2[i] == 0)
                {
                    banddivideresult[i] = 0;
                }
                else
                {
                    banddivideresult[i] = BandsDataD1[i] / BandsDataD2[i];
                }
                
            }
            return banddivideresult;
        }

    }
}
