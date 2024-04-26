using System;
using System.Collections;
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
        private NumStack nStack;
        private CalcHistory hist;
        private UserDefinedFunctions udf;
        int enterPressed;
        int numFloats;

        // constructor
        public Calculator() {
            DisplayString = "";
            enterPressed = 0;
            nStack = new NumStack();
            hist = new CalcHistory();
            udf = new UserDefinedFunctions();
            numFloats = 5;
        }

        //this method updates the displayString to the top value of the string stack
        public void updateDisplayString()
        {
            if (nStack.Count() > 0 && nStack.Peek() != "" && nStack.Peek() != ".")
            {
                DisplayString = Math.Round(double.Parse(nStack.Peek()), numFloats).ToString();
            }
            else
            {
                DisplayString = "";
            }
            if (nStack.Count() > 0 && nStack.Peek().Length > 0 && ((nStack.Peek()[nStack.Peek().Length - 1] == '.') || (numFloats == 0 && nStack.Peek().Contains('.')))) { DisplayString += "."; }
        }

        //This method will be called when a number is pressed, and will update the DisplayString
        public void pressNumber(char number)
        {
            //before pressing a Number, update the history
            hist.updateHistory(new CalcStatus(nStack, enterPressed));
            //we have to check if there is already a decimal
            if (number == '.' && enterPressed == 0 && nStack.Peek().Contains('.'))
            {
                return;
            }
            nStack.updateTop(number.ToString(), enterPressed);
            enterPressed = 0;
            updateDisplayString();
          
        }

        //This method will be called when enter is pressed, pushing the current value onto the stack display string won't be updated because the number wasn't modified.
        public void pressEnter()
        {
            if (enterPressed == 0)
            {
                //before pushing enter, update the history
                hist.updateHistory(new CalcStatus(nStack, enterPressed));

                nStack.Push(nStack.Peek());
                enterPressed = 2;
            }
        }

        //The way the operator works on the app he wants us to model is weird, I tried to imitate it with this goofy code
        public void pressOperator(string op)
        {
            //can't perform the operation if there isn't at least 1 value on the stack to add
            //if there isn't, do nothing
            if (op == "π")
            {
                hist.updateHistory(new CalcStatus(nStack, enterPressed));
                nStack.Push(Math.PI.ToString());
                updateDisplayString();
                return;
            }
            //call undo
            else if (op == "Undo")
            {
                CalcStatus history = hist.getHistory();
                if (history != null)
                {
                    nStack.stack = history.calcStack;
                    enterPressed = history.enterPressed;
                }
                updateDisplayString();
                return;
            }
            else if (op == "addFloat")
            {
                if (numFloats < 12)
                {
                    numFloats++;
                    updateDisplayString();
                }
                return;
            }
            else if (op == "removeFloat")
            {
                if (numFloats > 0)
                {
                    numFloats--;
                    updateDisplayString();
                }
                return;
            }
            else if (nStack.Count() > 0)
            {
                hist.updateHistory(new CalcStatus(nStack, enterPressed));
                double operand1 = double.Parse(nStack.Pop());
                /*
                 * 's' == sin()
                 * 'c' == cos()
                 * 't' == tan()
                 * 'S' == arcsin()
                 * 'C' == arccos()
                 * 'T' == arctan()
                 * '!' == x!
                 * 'r' == sqrt(x)
                 * 'R' == y root x
                 * "LOG" == log
                 * 'L' == ln
                 * 'p' == pi
                 * 'e' == e^x
                 * 'E' == x^y
                 */
                if ((op == "+" || op == "-" || op == "x" || op == "/" || op == "x^y" || op == "y√x") && nStack.Count() > 0)
                {
                    if (nStack.Count() > 0)
                    {
                        double operand2 = double.Parse(nStack.Pop());
                        switch (op)
                        {
                            case "+":
                                nStack.Push((operand1 + operand2).ToString());
                                break;
                            case "-":
                                nStack.Push((operand1 - operand2).ToString());
                                break;
                            case "x":
                                nStack.Push((operand1 * operand2).ToString());
                                break;
                            case "/":
                                nStack.Push((operand1 / operand2).ToString());
                                break;
                            case "x^y":
                                nStack.Push((Math.Pow(operand1, operand2)).ToString());
                                break;
                            case "y√x":
                                nStack.Push(Math.Pow(operand1, 1.0 / operand2).ToString());
                                break;
                        }
                    }
                }
                //otherwise, the operation only takes one operand
                else
                {
                    switch (op)
                    {
                        case "sin()":
                            nStack.Push(Math.Sin(operand1).ToString());
                            break;
                        case "cos()":
                            nStack.Push(Math.Cos(operand1).ToString());
                            break;
                        case "tan()":
                            nStack.Push(Math.Tan(operand1).ToString());
                            break;
                        case "arcsin()":
                            nStack.Push(Math.Asin(operand1).ToString());
                            break;
                        case "arccos()":
                            nStack.Push(Math.Acos(operand1).ToString());
                            break;
                        case "arctan()":
                            nStack.Push(Math.Atan(operand1).ToString());
                            break;
                        case "x!":
                            //we're only gonna do factorial on integers
                            if (operand1 % 1 == 0)
                            {
                                nStack.Push(Factorial((int)operand1).ToString());
                            }
                            else
                            {
                                nStack.Push(operand1.ToString());
                            }
                            break;
                        case "√x":
                            nStack.Push(Math.Sqrt(operand1).ToString());
                            break;
                        case "LOG":
                            nStack.Push(Math.Log10(operand1).ToString());
                            break;
                        case "LN":
                            nStack.Push(Math.Log(operand1).ToString());
                            break;
                        case "eⁿ":
                            nStack.Push(Math.Exp(operand1).ToString());
                            break;
                    }
                }
            }
            updateDisplayString();
            //setting enterPressed, so the next type on the calculator doesn't add to the current display, but creates a new value on the stack
            enterPressed = 1;
        }

        //This method will be called when clear is pressed -- deletes the current display string
        public void pressClear()
        {
            //update everything to default values
            DisplayString = "";
            hist = new CalcHistory();
            nStack = new NumStack();
            enterPressed = 0;
        }
        
        public void userDefinedFunction(string function)
        {
            double? returnValue = udf.CreateFunction(function, ref nStack.stack);
            enterPressed = 1;
        }
        public int Factorial(int f)
        {
                if (f == 0)
                    return 1;
                else
                    return f * Factorial(f - 1);
        }

        public IEnumerable<string> GetTopStackItems()
        {
            // Get the stack items
            List<string> stackItems = nStack.GetStackItems();

            // Return the top five items rounded to the same number of decimal places as DisplayString
            return stackItems
                .Where(item => !string.IsNullOrEmpty(item)) // Filter out empty strings
                .TakeLast(5)
                .Select(item => Math.Round(double.Parse(item), numFloats).ToString());
        }
    }
}
