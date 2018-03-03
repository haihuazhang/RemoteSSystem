using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 通过波段名索引波段号
    /// </summary>
    class GetBandByname
    {
        /// <summary>
        /// 索引方法
        /// </summary>
        /// <param name="Bandsname">波段名数组</param>
        /// <param name="name">波段名</param>
        /// <param name="bands">波段数</param>
        /// <returns>n为波段号</returns>
        public int getnumber(string[] Bandsname,string name,int bands)
        {
            int n=Int16.MinValue;
            for(int i=0;i<bands;i++)
                if(Bandsname[i]==name)
                {
                    n=i;
                    break;
                }
            return  n;
        }
    }
}
