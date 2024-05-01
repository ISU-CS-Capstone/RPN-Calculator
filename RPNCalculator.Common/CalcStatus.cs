using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNCalculator.Common
{
        /*
         * Author:  Craig Price
         * Description: This class stores a "snapshot" of the history of the calculator. Including the stack, whether enter has been pressed, and if there is an error.
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
    internal class CalcStatus
    {
        public Stack<string> calcStack;
        public int enterPressed;
        public bool error;

        public CalcStatus(NumStack nStack, int nEPressed, bool errorPressed)
        {
            calcStack = new Stack<string>(nStack.stack.Reverse());
            enterPressed = nEPressed;
            error = errorPressed;
        }


    }
}
