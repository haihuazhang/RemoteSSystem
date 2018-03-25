using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSystem
{
    class GetWinByName
    {
        public int WinNum;
        public int GetWinN(List<imageview> Wins, string windowname)
        {
            WinNum = Int16.MinValue;
            for (int i = 0; i < Wins.Count; i++)
            {
                if (windowname == Wins[i].windowname)
                {
                    WinNum = i;
                }
            }

            return WinNum;
        }
    }
}
