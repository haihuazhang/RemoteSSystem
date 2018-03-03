using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  System.IO;
using System.Windows.Forms;

namespace RemoteSystem
{
    class BsqToBil
    {
        
        public byte[] bits;
        public BsqToBil(int ColumnCounts, int LineCounts, int bands,int DataType)
        {
            bits = new byte[ColumnCounts * LineCounts * bands*DataType];
        }
        public bool bsqTobil(int ColumnCounts, int LineCounts,  int bands,int DataType, string BilPATH)
        {
            
             int totalpix = ColumnCounts * LineCounts *  bands;
            FileStream fs = new FileStream(BilPATH, FileMode.Create);

            if (totalpix > 0)
            {
                for (int i = 0; i < LineCounts; i++)
                {
                    for (int j = 0; j < bands; j++)
                    {
                        for (int k = 0; k < ColumnCounts; k++)
                        {
                            for (int p = 0; p < DataType; p++)
                                fs.WriteByte(bits[p + k * DataType + j * LineCounts * ColumnCounts * DataType + i * ColumnCounts * DataType]);
                        }
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
