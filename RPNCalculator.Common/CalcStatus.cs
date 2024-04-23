using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNCalculator.Common
{
    //new class to store the values of the current history
    internal class CalcStatus
    {
        public NumStack calcStack;
        public int enterPressed;

        public CalcStatus(NumStack nStack, int nEPressed)
        {
            calcStack = nStack;
            enterPressed = nEPressed;
        }


    }
}
