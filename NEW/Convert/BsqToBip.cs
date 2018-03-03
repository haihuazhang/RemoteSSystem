using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace RemoteSystem
{
    class BsqToBip
    {
        public byte[] bits;
        public BsqToBip(int ColumnCounts, int LineCounts, int bands,int DataType)
        {
            bits = new byte[ColumnCounts * LineCounts * bands*DataType];
        }
        public bool bsqTobip(int ColumnCounts, int LineCounts,  int bands,int DataType, string BipPATH)
        {
            
            int totalpix = ColumnCounts * LineCounts  * bands*DataType;
            FileStream fs = new FileStream(BipPATH, FileMode.Create);
            if (totalpix > 0)
            {
                for (int i = 0; i < LineCounts * ColumnCounts; i++)
                {

                    for (int k = 0; k < bands; k++)
                    {
                        for (int j = 0; j < DataType; j++)
                            fs.WriteByte(bits[j + k * LineCounts * ColumnCounts * DataType + i * DataType]);
                    }

                }
                fs.Flush();
                fs.Close();
                fs.Dispose();
                return true;
            }
            else
            {
                fs.Close();
                fs.Dispose();
                return false;

            }
            
            
           

        }

    }
}
