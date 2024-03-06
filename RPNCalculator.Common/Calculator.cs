using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using RPNCalculator.Common.Models;

namespace RPNCalculator.Common
{
    public class Calculator
    {
        //This string will be accessed by the frontend and will contain what should be displayed
        public string DisplayString { get; private set; }

        // Stack to hold numbers and results
        private RpnStack _rpnStack;

        // constructor
        public Calculator() {
            DisplayString = "";
            _rpnStack = new RpnStack();
        }

        //This method will be called when a number is pressed, and will update the DisplayString
        public string PressNumber(char number) { 
            DisplayString += number;
            return DisplayString;
        }

        //This method will be called when enter is pressed, pushing the current value onto the stack display string won't be updated because the number wasn't modified.
        public void PressEnter()
        {
            _rpnStack.Push(Double.Parse(DisplayString));
            DisplayString = "";
        }

        // Get the top two operators of the stack and perform the operation, set display string and push value onto stack
        public string PressOperator(char op)
        {
            Console.WriteLine(_rpnStack.Count());
            // Ensure there are at least two values on the stack for the operation
            if (_rpnStack.Count() < 2)
            {
                DisplayString = "Error: Insufficient values";
                return DisplayString;
            }

            double operand2 = _rpnStack.Pop(); // Pop the top two operands
            double operand1 = _rpnStack.Pop();
            double result = 0;

            switch (op)
            {
                case '+':
                    result = operand1 + operand2;
                    break;
                case '-':
                    result = operand1 - operand2;
                    break;
                case '*':
                    result = operand1 * operand2;
                    break;
                case '/':
                    if (operand2 == 0) // Check for division by zero
                    {
                        DisplayString = "Error: Divide by zero";
                        return DisplayString;
                    }
                    result = operand1 / operand2;
                    break;
                default:
                    DisplayString = "Error: Invalid operator";
                    return DisplayString;
            }

            DisplayString = result.ToString();
            _rpnStack.Push(result); // Push the result back onto the stack
            
            return DisplayString;
        }

        // erase all stack and display string
        public void Clear()
        {
            DisplayString = "";
            while (_rpnStack.Count() > 0)
            {
                _rpnStack.Pop();
            }
        }
    }
}
