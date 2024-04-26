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
        public Stack<string> calcStack;
        public int enterPressed;

        public CalcStatus(NumStack nStack, int nEPressed)
        {
            calcStack = new Stack<string>(nStack.stack.Reverse());
            enterPressed = nEPressed;
        }


    }
}
