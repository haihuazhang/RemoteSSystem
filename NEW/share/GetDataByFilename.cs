using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    /// <summary>
    /// 通过文件名索引数据编号
    /// </summary>
    class GetDataByFilename
    {
        /// <summary>
        /// 索引方法
        /// </summary>
        /// <param name="readmore">数据流</param>
        /// <param name="FileName">文件名</param>
        /// <returns>record数据编号</returns>
        public int getnumber(List<read> readmore, string FileName)
        {
            int record = Int16.MinValue;
            for (int i = 0; i < readmore.Count; i++)

                if (readmore[i].FileName == FileName)
                    record = i;
            return record;
        }
    }
}
