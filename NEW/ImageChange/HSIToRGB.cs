using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    class HSIToRGB
    {
        public HSIToRGB(int record)
        {
            isHSI = true;
            rd = Form1.boduan[record];
            double Hmin = double.MaxValue; double Hmax = double.MinValue;
            double Smin = double.MaxValue; double Smax = double.MinValue;
            double Imin = double.MaxValue; double Imax = double.MinValue;
            for (int i = 0; i < rd.ColumnCounts * rd.LineCounts; i++)
            {
                if (Hmin > rd.BandsDataD[0, i])
                    Hmin = rd.BandsDataD[0, i];
                if (Hmax < rd.BandsDataD[0, i])
                    Hmax = rd.BandsDataD[0, i];
                if (Smin > rd.BandsDataD[1, i])
                    Smin = rd.BandsDataD[1, i];
                if (Smax < rd.BandsDataD[1, i])
                    Smax = rd.BandsDataD[1, i];
                if (Imin > rd.BandsDataD[2, i])
                    Imin = rd.BandsDataD[2, i];
                if (Hmax < rd.BandsDataD[2, i])
                    Hmax = rd.BandsDataD[2, i];
            }
            if (Hmax > 360 || Hmin < 0 || Smax > 1 || Smin < 0 || Imax > 1 || Imin < 0)
                isHSI = false;
            else
            {
                HSI = rd.BandsDataD;
                this.ColumnCounts = rd.ColumnCounts;
                this.LineCounts = rd.LineCounts;
            }
        }
        public bool isHSI;
        read rd;
        private double[,] HSI;
        private int ColumnCounts, LineCounts;
        public double[,] evaluateRGB()
        {
            double[,] RGB = new double[rd.bands, rd.ColumnCounts * rd.LineCounts];
            for (int i = 0; i < rd.ColumnCounts * rd.LineCounts; i++)
            {
                if (HSI[0, i] > 0 && HSI[0, i] <= 120)
                {
                    RGB[2, i] = HSI[2,i]* (1 - HSI[1, i]);
                    RGB[0, i] = HSI[2,i]* (1 + (HSI[1, i] * Math.Cos(HSI[0, i]/180*Math.PI)) / Math.Cos((60 - HSI[0, i])/180*Math.PI));
                    RGB[1, i] = HSI[2,i]*3 - (RGB[0, i] + RGB[2, i]);
                }
                else if (HSI[0, i] > 120 && HSI[0, i] <= 240)
                {
                    //HSI[0, i] = HSI[0, i] - 120;
                    RGB[0, i] = HSI[2, i] * (1 - HSI[1, i]);
                    RGB[1, i] = HSI[2,i]*(1 + (HSI[1, i] * Math.Cos((HSI[0, i]-120) / 180 * Math.PI)) / Math.Cos((180 - HSI[0, i]) / 180 * Math.PI));
                    RGB[2, i] = HSI[2,i]*3 - (RGB[0, i] + RGB[1, i]);
                }
                else
                {
                    //HSI[0, i] = HSI[0, i] - 240;
                    RGB[1, i] = HSI[2, i] * (1 - HSI[1, i]);
                    RGB[2, i] = HSI[2,i]*(1 + (HSI[1, i] * Math.Cos((HSI[0, i]-240) / 180 * Math.PI)) / Math.Cos((300 - HSI[0, i]) / 180 * Math.PI));
                    RGB[0, i] = HSI[2,i]*3 - (RGB[1, i] + RGB[2, i]);
                }
                RGB[0, i] *= 255;
                RGB[1, i] *= 255;
                RGB[2, i] *= 255;
            }
            return RGB;
        }
        public read Getresult()
        {
            read rd1=new read();
            rd1.ColumnCounts = this.ColumnCounts;
            rd1.LineCounts = this.LineCounts;
            rd1.bands = 3;
            rd1.BandsDataD = evaluateRGB();
            rd1.Bandsname = new string[rd.bands];
            rd1.BandsData = new int[rd.bands, ColumnCounts * LineCounts];
            for (int i = 0; i < rd1.bands; i++)
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                    rd1.BandsData[i, j] = (int)rd1.BandsDataD[i, j];
            rd1.DataType = 4;
            return rd1;
        
        }
    }
}
