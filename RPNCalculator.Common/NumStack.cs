using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNCalculator.Common
{
    internal class NumStack
    {
        private Stack<string> stack;
        public NumStack()
        {
            stack = new Stack<string>();
            stack.Push("");
        }

        public void Push(string numString)
        {
            stack.Push(numString);
        }

        public string Pop()
        {
            return stack.Pop();
        }

        public int Count()
        {
            return stack.Count;
        }

        public void updateTop(string update, bool overwrite) 
        { 
            if (stack.Count > 0)
            {
                if (!overwrite)
                {
                    stack.Push(stack.Pop() + update);
                }
                else
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

        // Method to get the stack items as a list of strings
        public List<string> GetStackItems()
        {
            // Convert the stack items to a list of strings
            List<string> stackList = new List<string>(stack);
            stackList.Reverse(); // Reverse the list to maintain stack order
            return stackList;
            }

        public string Peek()
        {
            return stack.Peek();
        }
    }
}
