using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 显示数据的初始化，主要用于不同方法的图像增强中
    /// </summary>
    class BandpixelInitialize
    {
        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="bandstemp">暂存的显示数据</param>
        /// <param name="bandints">0-255值域显示数据</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="bands">波段数</param>
        public void Initialization(out int[,] bandstemp, int[,] bandints, int ColumnCounts, int LineCounts, int bands)
        {
            bandstemp = new int[bands, ColumnCounts * LineCounts];
            for (int i = 0; i < bands; i++)
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                    bandstemp[i, j] = bandints[i, j];
        }
    }
}
