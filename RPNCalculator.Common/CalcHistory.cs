using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPNCalculator.Common
{
    internal class CalcHistory
    {
        List<CalcStatus> History;
        int historySize = 10;

        public CalcHistory()
        {
            History = new List<CalcStatus>();
        }

        public void updateHistory(CalcStatus x)
        {
            History.Add(x);
            if (History.Count > historySize)
            {
                History.RemoveAt(0);
            }
        }

        public CalcStatus? getHistory()
        {
            //return the last object pushed onto the stack and remove it
            if (History.Count > 0)
            {
                CalcStatus returnVal = History[History.Count - 1];
                History.RemoveAt(History.Count - 1);
                return returnVal;
            }
            else
            {
                return null;
            }
        }

    }
}
