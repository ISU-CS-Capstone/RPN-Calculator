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
        /*
         * Author: Craig Price
         * Description: Class level variables we have to keep track of
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public string DisplayString { get; private set; }

        private NumStack nStack;
        private CalcHistory hist;
        private UserDefinedFunctions udf;
        int enterPressed;
        int numFloats;
        bool error;

        /*
         * Author: Craig Price
         * Description: This is the default constructor for our calculator class
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public Calculator() {
            DisplayString = "";
            enterPressed = 0;
            nStack = new NumStack();
            hist = new CalcHistory();
            udf = new UserDefinedFunctions();
            numFloats = 5;
            error = false;
        }

        /*
         * Author: Craig Price
         * Description: This method updates the display string from the top of the stack to keep it looking nice and presentable.
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public void updateDisplayString()
        {
            if (nStack.Count() > 0 && nStack.Peek() != "" && nStack.Peek() != "." && !error)
            {
                DisplayString = Math.Round(double.Parse(nStack.Peek()), numFloats).ToString();
            }
            else if (error)
            {
                DisplayString = "ERROR";
            }
            else 
            {
                DisplayString = "";
            }
            if (nStack.Count() > 0 && nStack.Peek().Length > 0 && ((nStack.Peek()[nStack.Peek().Length - 1] == '.') || (numFloats == 0 && nStack.Peek().Contains('.')))) { DisplayString += "."; }
        }

        /*
         * Author: Craig Price
         * Description: This method is called when a number is pressed. it updates the top string in the stack with the new number
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public void pressNumber(char number)
        {
            //before pressing a Number, update the history
            hist.updateHistory(new CalcStatus(nStack, enterPressed, error));
            //we have to check if there is already a decimal
            if (number == '.' && enterPressed == 0 && nStack.Peek().Contains('.'))
            {
                return;
            }
            nStack.updateTop(number.ToString(), enterPressed);
            error = false;
            enterPressed = 0;
            updateDisplayString();
          
        }

        /*
         * Author: Craig Price
         * Description: This method is called when enter is pressed on the calculator. It pushes the value onto the stack and allows the user to start modifying a new value.
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public void pressEnter()
        {
            if ((enterPressed == 0 || enterPressed == 1) && !error)
            {
                //before pushing enter, update the history
                hist.updateHistory(new CalcStatus(nStack, enterPressed, error));

                nStack.Push(nStack.Peek());
                enterPressed = 2;
            }
        }

        /*
         * Author: Craig Price
         * Description: This method determines what operator was pressed, and executes the correct code
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public void pressOperator(string op)
        {
                if ((op == "π" || op == "pi") && !error)
                {
                    hist.updateHistory(new CalcStatus(nStack, enterPressed, error));
                    nStack.Push(Math.PI.ToString());
                    updateDisplayString();
                    enterPressed = 1;
                    return;
                }
                if (op == "e" && !error)
                {
                    hist.updateHistory(new CalcStatus(nStack, enterPressed, error));
                    nStack.Push(Math.E.ToString());
                    updateDisplayString();
                    enterPressed = 1;
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
                        error = history.error;
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
                else if (op == "+/-" && nStack.Count() > 0 && nStack.Peek() != "" && nStack.Peek() != "." && !error)
                {
                    nStack.Push((double.Parse(nStack.Pop())*-1).ToString());
                    enterPressed = 0;
                    updateDisplayString();
                    return;
                }
                else if (nStack.Count() > 0 && nStack.Peek() != "" && nStack.Peek() != "." && !error)
                {

                    hist.updateHistory(new CalcStatus(nStack, enterPressed, error));
                    double operand1 = double.Parse(nStack.Pop());

                    if ((op == "+" || op == "-" || op == "x" || op == "/" || op == "x^y" || op == "y√x" || op == "yROOTx") &&
                        nStack.Count() > 0)
                    {
                        if (nStack.Count() > 0)
                        {
                            double operand2 = double.Parse(nStack.Pop());
                            switch (op)
                            {
                                case "+":
                                    nStack.Push((operand2 + operand1).ToString());
                                    break;
                                case "-":
                                    nStack.Push((operand2 - operand1).ToString());
                                    break;
                                case "x":
                                    nStack.Push((operand2 * operand1).ToString());
                                    break;
                                case "/":
                                    if (operand1 == 0)
                                    {
                                        error = true;
                                        enterPressed = 1;
                                        updateDisplayString();
                                        return;
                                    }
                                    nStack.Push((operand2 / operand1).ToString());
                                    break;
                                case "x^y":
                                    nStack.Push((Math.Pow(operand2, operand1)).ToString());
                                    break;
                                case "y√x":
                                case "yROOTx":
                                    if (operand1 == 0)
                                    {
                                        error = true;
                                        enterPressed = 1;
                                        updateDisplayString();
                                        return;
                                    }
                                    nStack.Push(Math.Pow(operand1, 1.0 / operand2).ToString());
                                    break;
                            }
                        }
                    }

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
                                if (operand1 < -1 || operand1 > 1)
                                {
                                    error = true;
                                    enterPressed = 1;
                                    updateDisplayString();
                                    return;
                                }
                                nStack.Push(Math.Asin(operand1).ToString());
                                break;
                            case "arccos()":
                                if (operand1 < -1 || operand1 > 1)
                                {
                                    error = true;
                                    enterPressed = 1;
                                    updateDisplayString();
                                    return;
                                }
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
                            case "ROOTx":
                                if (operand1 < 0)
                                {
                                    error = true;
                                    enterPressed = 1;
                                    updateDisplayString();
                                    return;
                                }
                                nStack.Push(Math.Sqrt(operand1).ToString());
                                break;
                            case "LOG":
                                if (operand1 <= 0)
                                {
                                    error = true;
                                    enterPressed = 1;
                                    updateDisplayString();
                                    return;
                                }
                                nStack.Push(Math.Log10(operand1).ToString());
                                break;
                            case "ln":
                            case "LN":
                                if (operand1 <= 0)
                                {
                                    error = true;
                                    enterPressed = 1;
                                    updateDisplayString();
                                    return;
                                }
                                nStack.Push(Math.Log(operand1).ToString());
                                break;
                            case "eⁿ":
                            case "e^n":
                                nStack.Push(Math.Exp(operand1).ToString());
                                break;
                            case "x^2":
                                nStack.Push(Math.Pow(operand1,2).ToString());
                                break;
                            default:
                                nStack.Push(operand1.ToString());
                                hist.getHistory();
                                enterPressed = 0;
                                return;
                        }
                    }
                }

                updateDisplayString();
                //setting enterPressed, so the next type on the calculator doesn't add to the current display, but creates a new value on the stack
                enterPressed = 1;
        }

        /*
         * Author: Craig Price
         * Description: This method will be called when clear is pressed. Sets everything to its default value
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public void pressClear()
        {
            //update everything to default values
            DisplayString = "";
            hist = new CalcHistory();
            nStack = new NumStack();
            enterPressed = 0;
            error = false;
        }

        /*
         * Author: Craig Price
         * Description: This method allows the user to create a userdefined function using the UDF class
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public void userDefinedFunction(string function)
        {
            hist.updateHistory(new CalcStatus(nStack, enterPressed, error));
            double? returnValue = udf.CreateFunction(function, ref nStack.stack);
            if (returnValue != null)
            {
                nStack.Push(returnValue.ToString());
                updateDisplayString();
                enterPressed = 1;
            }
        }

        /*
         * Author: Craig Price
         * Description: This performs factorial on an integer
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public int Factorial(int f)
        {
                if (f == 0)
                    return 1;
                else
                    return f * Factorial(f - 1);
        }

        /*
         * Author: Victoria Weir and Craig Price
         * Description: This gets the top stack from the stack
         * Responsibility: Backend work for the Calculator
         * Certification: I certify that I wrote this code myself.
         */
        public IEnumerable<string> GetTopStackItems()
        {
            // Get the stack items
            List<string> stackItems = nStack.GetStackItems();
            if (stackItems.Count > 0 && !error) { stackItems.RemoveAt(stackItems.Count - 1); }

            // Return the top five items rounded to the same number of decimal places as DisplayString
            return stackItems
                .Where(item => !string.IsNullOrEmpty(item)) // Filter out empty strings
                .TakeLast(5)
                .Select(item => Math.Round(double.Parse(item), numFloats).ToString());
        }
    }
}
