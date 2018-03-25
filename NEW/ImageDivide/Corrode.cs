using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 腐蚀
    /// </summary>
    public class Corrode
    {
        /// <summary>
        /// 结构
        /// </summary>
        /// <param name="structure"></param>
        public Corrode(int[,] structure)
        {
            this.structure = structure;
        }
        /// <summary>
        /// 腐蚀结果
        /// </summary>
        public int [,] Result;
        /// <summary>
        /// 结构元素，原点为左下角点
        /// </summary>   
        public int[,] structure;
        public int[,]  corrode(int [,] BandsDataD, int bands, int ColumnCounts, int LineCounts)
        {
            Result = new int [bands, ColumnCounts * LineCounts];
            for (int i = 0; i < bands; i++)
            {
                for (int j = 0; j < LineCounts; j++)
                {
                    for (int k = 0; k < ColumnCounts; k++)
                    {
                        /// <summary>
                        /// 图像边缘处理
                        /// <summary>
                        if (j == LineCounts - 1 || k == ColumnCounts - 1||j==0||k==0)
                        {
                            Result[i, j * ColumnCounts + k] = BandsDataD[i, j * ColumnCounts + k];
                        }
                        else
                        {
                            
                            //将结构元素与二值图像进行&运算，如果都为1则原点要素为1，否则为0
                            int count=0;
                            for(int p=-1;p<2;p++)
                            {
                                for(int q=-1;q<2;q++)
                                {
                                    if(structure[p+1,q+1]==1&&BandsDataD[i,(j+p)*ColumnCounts+k+q]!=1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        count++;
                                    }
                                }
                            }
                            if(count==9)
                            {
                                Result[i, j * ColumnCounts + k] = 1;
                            }
                            else
                            {
                                Result[i, j * ColumnCounts + k] = 0;
                            }
                        }
                    }
                }
            }
            return Result;
}

    }
}
