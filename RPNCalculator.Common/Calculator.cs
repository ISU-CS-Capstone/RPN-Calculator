using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RPNCalculator.Common
{
    public class Calculator
    {
        //This string will be accessed by the frontend and will contain what should be displayed
        public string DisplayString { get; private set; }

        //Stack to hold all of the numbers
        private NumStack nStack;

        //this boolean keeps track of if enter has been pressed, to determine whether to update the DisplayString
        bool enterPressed;

        //simple constructor
        public Calculator(){
            DisplayString = "";
            nStack = new NumStack();
            enterPressed = false;
        }

        //this method updates the displayString to the top value of the string stack
        public void updateDisplayString()
        {
            DisplayString = nStack.Peek();
        }

        //This method will be called when a number is pressed, and will update the DisplayString
        public void pressNumber(char number)
        {
            nStack.updateTop(number.ToString(), enterPressed);
            enterPressed = false;
            updateDisplayString();
        }

        //This method will be called when enter is pressed, pushing the current value onto the stack display string won't be updated because the number wasn't modified.
        public void pressEnter()
        {
            if (!enterPressed)
            {
                nStack.Push(DisplayString);
                enterPressed = true;
            }
        }

        //The way the operator works on the app he wants us to model is weird, I tried to imitate it with this goofy code
        public void pressOperator(char op)
        {
            //can't perform the operation if there isn't at least 1 value on the stack to add
            //if there isn't, do nothing
            if (op == 'p')
            {
                nStack.Push(Math.PI.ToString());
            }
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
             * 'l' == log
             * 'L' == ln
             * 'p' == pi
             * 'e' == e^x
             * 'E' == x^y
             */
            if (op == '+' || op == '-' || op == '*' || op == '/' || op == 'E' || op == 'R')
            {
                double operand2 = double.Parse(nStack.Pop());
                switch (op)
                {
                    case '+':
                        nStack.Push((operand1 + operand2).ToString());
                        break;
                    case '-':
                        nStack.Push((operand1 - operand2).ToString());
                        break;
                    case '*':
                        nStack.Push((operand1 * operand2).ToString());
                        break;
                    case '/':
                        nStack.Push((operand1 / operand2).ToString());
                        break;
                    case 'E':
                        nStack.Push((Math.Pow(operand1,operand2)).ToString());
                        break;
                    case 'R':
                        nStack.Push(Math.Pow(operand1, 1.0 / operand2).ToString());
                        break;
                }
            }
            //otherwise, the operation only takes one operand
            else
            {
                switch (op)
                {
                    case 's':
                        nStack.Push(Math.Sin(operand1).ToString());
                        break;
                    case 'c':
                        nStack.Push(Math.Cos(operand1).ToString());
                        break;
                    case 't':
                        nStack.Push(Math.Tan(operand1).ToString());
                        break;
                    case 'S':
                        nStack.Push(Math.Asin(operand1).ToString());
                        break;
                    case 'C':
                        nStack.Push(Math.Acos(operand1).ToString());
                        break;
                    case 'T':
                        nStack.Push(Math.Atan(operand1).ToString());
                        break;
                    case '!':
                        //we're only gonna do factorial on integers
                        if (operand1 % 1 == 0)
                        {
                            nStack.Push(Factorial((int)operand1).ToString());
                        }
                        break;
                    case 'r':
                        nStack.Push(Math.Sqrt(operand1).ToString());
                        break;
                    case 'l':
                        nStack.Push(Math.Log10(operand1).ToString());
                        break;
                    case 'L':
                        nStack.Push(Math.Log(operand1).ToString());
                        break;
                    case 'e':
                        nStack.Push(Math.Exp(operand1).ToString());
                        break;
                }
            }
            updateDisplayString();
            //setting enterPressed, so the next type on the calculator doesn't add to the current display, but creates a new value on the stack
            enterPressed = true;
        }

        //This method will be called when clear is pressed -- deletes the current display string
        public void pressClear()
        {
            DisplayString = "";
        }
        
        public int Factorial(int f)
        {
                if (f == 0)
                    return 1;
                else
                    return f * Factorial(f - 1);
        }
    }
}
