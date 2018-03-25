using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    class perHistogram
    {
        public double max = double.MinValue;
        public double min=double.MaxValue;
        public double stretch=0;
        //public int DataType;

        /// <summary>
        /// 1.0版本直方图（已弃用!）
        /// </summary>
        /// <param name="BandsData"></param>
        /// <param name="bandtotalnum"></param>
        /// <param name="i"></param>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public bool histogram(int[,]BandsData,int bandtotalnum,int i,out int[] pixel)
        {

            for (int j = 0; j < bandtotalnum; j++)
            {
                if (max < BandsData[i, j])
                {
                    max = BandsData[i, j];
                }
            }
            if ((int)max <= 255)
            {
                max = 255;
            }

            pixel = new int[(int)max+1];
            int p = 0;
            


            for (int j = 0; j < bandtotalnum; j++)
            {
                pixel[BandsData[i, j]]++;
            }
            
            for (int j = 0; j <= (int)max; j++)
            {
                if (pixel[j] == 0)
                {
                    p++;
                }
            }
            if (p == (int)max)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 1.0版本累计直方图（已弃用）
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="overlay"></param>
        public void totalhistogram(int[] pixel, out int[] overlay)
        {
            if ((int)max <= 255)
            {
                max = 255;
            }
 
                overlay = new int[(int)max+1];
                for (int j = 0; j <= (int)max; j++)
                {
                    for (int k = 0; k <= j; k++)
                    {
                        overlay[j] += pixel[k];
                    }
                }
            
            
            //for (int j = 0; j < 256; j++)
            //{
            //    overlay[j] /= 10;
            //}

        }
        /// <summary>
        /// 2.0版本直方图（分块）
        /// </summary>
        /// <param name="BandsDataD"></param>
        /// <param name="bandtotalnum"></param>
        /// <param name="i"></param>
        /// <param name="pixel"></param>
        public void histogram2(double[,] BandsDataD, int bandtotalnum, int i, out int[] pixel)
        {
            for (int j = 0; j < bandtotalnum; j++)
            {
                if (max < BandsDataD[i, j])
                {
                    max = BandsDataD[i, j];
                }
                else if (min > BandsDataD[i, j])
                {
                    min = BandsDataD[i, j];
                }
            }
            stretch = max - min;
            if (stretch == 0)
            {
                stretch = 1;
            }

            pixel = new int[256];
            //将数据进行分块，max-min分成255块，并将原数据合并，形成灰度直方图

            for (int j = 0; j < bandtotalnum; j++)
            {
                pixel[(int)Math.Ceiling((BandsDataD[i, j] - min) / stretch*255 )]++;
            }
             
        }
        /// <summary>
        /// 2.0版本累计直方图
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="overlay"></param>
        public void totalhistogram2(int[] pixel, out int[] overlay)
        {
            overlay = new int[256];
            for (int j = 0; j <= 255; j++)
            {
                for (int k = 0; k <= j; k++)
                {
                    overlay[j] += pixel[k];
                }
            }
        }

    }
}
