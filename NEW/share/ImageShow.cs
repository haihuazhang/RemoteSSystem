using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RemoteSystem
{
    /// <summary>
    /// 显示图像类
    /// </summary>
    class ImageShow
    {
        /// <summary>
        /// Bitmap方法显示图像
        /// </summary>
        /// <param name="bandints">0-255值域像素数据</param>
        /// <param name="ColumnCounts">列数</param>
        /// <param name="LineCounts">行数</param>
        /// <param name="Rband">R波段编号</param>
        /// <param name="Gband">G波段编号</param>
        /// <param name="Bband">B波段编号</param>
        /// <param name="map">map变量</param>
        public void showimage(int[,] bandints, int ColumnCounts, int LineCounts, int key, Bitmap map)
        {
            int Rband = 0; int Gband = 0; int Bband = 0;
            if (key == 1)
            { }
            else
            {
                Rband = 0;
                Gband = 1;
                Bband = 2;
            }
            for (int i = 0; i < LineCounts; i++)
            {
                for (int j = 0; j < ColumnCounts; j++)
                {
                    /// <summary>
                    ///像素值提取
                    /// <summary>
                    int r = bandints[Rband, i * ColumnCounts + j];
                    int g = bandints[Gband, i * ColumnCounts + j];
                    int b = bandints[Bband, i * ColumnCounts + j];
                    map.SetPixel(j, i, Color.FromArgb(r, g, b));
                    //map.SetPixel(j,i,Color.)
                }
            }

        }
    }
}
