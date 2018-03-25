using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    class Kmean
    {
        /// <summary>
        /// 原始波段数据
        /// </summary>
        double[][] BandsDataD;
        /// <summary>
        /// 分类数
        /// </summary>
        int ClassNum;
        /// <summary>
        /// 行列
        /// </summary>
        int ColumnCounts,LineCounts;
        /// <summary>
        /// 波段数
        /// </summary>
        int bands;
        /// <summary>
        /// 迭代次数
        /// </summary>
        int times;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="N"></param>
        /// <param name="ClassNum"></param>
        /// <param name="times"></param>
        public Kmean(int N,int ClassNum,int times)
        {
            
            this.bands = Form1.boduan[N].bands;
            this.ColumnCounts = Form1.boduan[N].ColumnCounts;
            this.LineCounts = Form1.boduan[N].LineCounts;
            this.ClassNum = ClassNum;
            this.times =times;
            this.BandsDataD = new double[bands][];
            for (int i = 0; i < bands; i++)
            {
                this.BandsDataD[i] = new double[ColumnCounts * LineCounts];
                for (int j = 0; j < ColumnCounts * LineCounts; j++)
                {
                    this.BandsDataD[i][j] = Form1.boduan[N].BandsDataD[i, j];
                }
            }
        }
        /// <summary>
        /// 反复迭代得到分类结果
        /// </summary>
        /// <returns></returns>
        public double[] operate()
        {
            int record = 0;
            //获取起始类中心
            double[][] CurrentCentre=InitialCentre();
            //结果数组
            double[] result = new double[ColumnCounts*LineCounts];
            //上一迭代类中心
            double[][] LastCentre = new double[ClassNum][];
            ///上一迭代与现在类中心重复
            LastCentre = CurrentCentre;
            //聚类
            result = gatherToCentre(CurrentCentre);
            //计算新类中心
            CurrentCentre = CreateNewCentre(result);
            //迭代
            while (record <= times)
            {
                record++;
                
                if (!judgeCentre(LastCentre, CurrentCentre))
                {
                    result = gatherToCentre(CurrentCentre);
                   
                    LastCentre = CurrentCentre;
                    CurrentCentre = CreateNewCentre(result);
                }
                else
                {
                    break;
                }
            }
            //分类结果加1
            for (int i = 0; i < ColumnCounts * LineCounts; i++)
            {
                result[i]++;
            }
            return result;
        }

        /// <summary>
        /// 初始化其实类中心
        /// </summary>
        /// <returns></returns>
        public double[][] InitialCentre()
        {
            //每个类的总像素数
            int averageNum = this.ColumnCounts*this.LineCounts/this.ClassNum;
            //类中心各波段值
            double[][] centre = new double[ClassNum][];
            for(int i=0;i<ClassNum;i++)
            {
                centre[i] = new double[bands];
                double[] sum = new double[bands];
                sum.Initialize();

                for(int j=0;j<bands;j++)
                {
                    for(int k=i*averageNum;k<(i+1)*averageNum;k++)
                    {
                        sum[j]+=BandsDataD[j][k];
                    }
                    centre[i][j] = sum[j] / averageNum;
                }
            }
            return centre;
        }
        /// <summary>
        /// 聚类函数
        /// </summary>
        /// <param name="CurrentCentre">当前类中心</param>
        /// <returns></returns>
        public double[] gatherToCentre(double[][] CurrentCentre)
        {
            //聚类结果
            double[] ClassResult = new double[ColumnCounts * LineCounts];
            for (int i = 0; i < ColumnCounts * LineCounts; i++)
            {
                double[] distance = new double[ClassNum];
                double[] data = new double[bands];
                for (int j = 0; j < bands; j++)
                {
                    data[j] = BandsDataD[j][i];
                }

                for (int k = 0; k < ClassNum; k++)
                {
                    distance[k] = Euidistance(data, CurrentCentre[k]);
                }

                double min = Double.MaxValue;
                int ClassType = 0;
                for (int k = 0; k < ClassNum; k++)
                {
                    if (min > distance[k])
                    {
                        //最小距离的类中心作为类别
                        min = distance[k];
                        ClassType = k;
                    }
                }

                ClassResult[i] = ClassType;
            }
            return ClassResult;
            
        }
        /// <summary>
        /// 计算新类中心
        /// </summary>
        /// <param name="ClassResult"></param>
        /// <returns></returns>
        public double[][] CreateNewCentre(double[] ClassResult)
        {
            double[][] NewCenter = new double[ClassNum][];
            for (int i = 0; i < ClassNum; i++)
            {
                NewCenter[i] = new double[bands];
                double[]  sum = new double[bands];
                for (int k = 0; k < bands; k++)
                {
                    int Classpixnum=0;
                    for (int j = 0; j < ColumnCounts * LineCounts; j++)
                    {
                        if (i == ClassResult[j])
                        {
                            Classpixnum++;
                            sum[k] += BandsDataD[k][j];
                        }
                    }
                    NewCenter[i][k] = sum[k] / Classpixnum;
                }
            }
            return NewCenter;
        }
        /// <summary>
        /// 计算欧式距离（像素点与类中心）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="centreValue"></param>
        /// <returns></returns>
        public double Euidistance(double[] data, double[] centreValue)
        {

            double distance = 0;
            for (int i = 0; i < bands; i++)
            {
                distance += Math.Pow(data[i] - centreValue[i],2);
                distance /= Math.Sqrt(distance);
            }
            return distance;
        }
        /// <summary>
        /// 判断是否收敛（条件是上一迭代类中心和当前类中心的欧式距离不大于1）
        /// </summary>
        /// <param name="LastCentre"></param>
        /// <param name="CurrentCentre"></param>
        /// <returns></returns>
        public bool judgeCentre(double[][] LastCentre,double[][] CurrentCentre)
        {
            bool isequal = true;
            for (int i = 0; i < ClassNum; i++)
            {
                double distance = Euidistance(LastCentre[i], CurrentCentre[i]);
                if (distance >= 1)
                {
                    isequal = false; 
                    break;
                }
            }
            return isequal;
        }
        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        public read GetResult()
        {
            read rd = new read();
            rd.bands=1;
            rd.ColumnCounts=ColumnCounts;
            rd.LineCounts=LineCounts;
            rd.DataType=4;
            double[] temp =operate();
            rd.BandsDataD = new double[rd.bands, rd.ColumnCounts * rd.LineCounts];
            rd.BandsData =new int[rd.bands,rd.ColumnCounts*rd.LineCounts];
            for(int i=0;i<rd.bands;i++)
            {
                for(int j=0;j<ColumnCounts*LineCounts;j++)
                {
                      rd.BandsDataD[i,j] = temp[j];
                      rd.BandsData[i, j] = (int)rd.BandsDataD[i, j];
                }

            }
            return rd;
        }
    }
}
