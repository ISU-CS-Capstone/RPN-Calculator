using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNCalculator.Common
{
    internal class NumStack
    {
        public Stack<string> stack;

        /*
         * Author: Craig Price
         * Description: Default Constructor for the NumStack
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public NumStack()
        {
            stack = new Stack<string>();
            stack.Push("");
        }

        /*
         * Author: Craig Price
         * Description: This method simply pushes a string onto the stack
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public void Push(string numString)
        {
            stack.Push(numString);
        }

        /*
         * Author: Craig Price
         * Description: This method simply pops a value from the numstack
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public string Pop()
        {
            return stack.Pop();
        }

        /*
         * Author: Craig Price
         * Description: This method counts the number of values in the numstack
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public int Count()
        {
            return stack.Count;
        }

        /*
         * Author: Craig Price
         * Description: This method determines what needs to happen with the top of the stack, then updates it.
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public void updateTop(string update, int overwrite) 
        { 
            //overwrite 0 = enter not pressed -> edit top of stack (append to the end of string at the top of the stack)
            //overwrite 1 = operation pressed -> push the new value on top of previous values
            //overwrite 2 = enter pressed -> pop the old top, and push the new top on
            if (stack.Count > 0)
            {
                if (overwrite == 0)
                {
                    stack.Push(stack.Pop() + update);
                }
                else if (overwrite == 1)
                {
                    stack.Push(update);
                }
                else if (overwrite == 2)
                {
                    stack.Pop();
                    stack.Push(update);
                }
            }
            else
            {
                stack.Push(update);
            }
        }

        /*
         * Author: Victoria Weir
         * Description: This creates a string of all stack items
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public List<string> GetStackItems()
        {
            // Convert the stack items to a list of strings
            List<string> stackList = new List<string>(stack);
            stackList.Reverse(); // Reverse the list to maintain stack order
            return stackList;
            }

        /*
         * Author: Craig Price
         * Description: This method peeks at the top of the stack
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public string Peek()
        {
            return stack.Peek();
        }
    }
}
