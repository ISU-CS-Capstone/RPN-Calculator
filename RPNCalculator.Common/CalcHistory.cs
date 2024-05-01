using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPNCalculator.Common
{
    /*
         * Author: and Craig Price
         * Description: This class holds a List of the most recent 10 histories.
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
    internal class CalcHistory
    {
        List<CalcStatus> History;
        int historySize = 10;

        /*
         * Author: Craig Price
         * Description: Default constructor for calculatorHistory
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public CalcHistory()
        {
            History = new List<CalcStatus>();
        }

        /*
         * Author: Craig Price
         * Description: This method adds a new Calculator status to the history
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public void updateHistory(CalcStatus x)
        {
            History.Add(x);
            if (History.Count > historySize)
            {
                History.RemoveAt(0);
            }
        }

        /*
         * Author: Craig Price
         * Description: This method returns and removes the most recent history
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
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
