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

        //This method will be called when a number is pressed, and will update the DisplayString
        public string pressNumber(char number) { 
            if (enterPressed)
            {
                DisplayString = "" + number;
                enterPressed = false;
            }
            else
            {
                DisplayString += number;
            }
            return DisplayString;
        }

        //This method will be called when enter is pressed, pushing the current value onto the stack display string won't be updated because the number wasn't modified.
        public void pressEnter()
        {
            nStack.Push(DisplayString);
            DisplayString = "";
            enterPressed = true;
        }

        //The way the operator works on the app he wants us to model is weird, I tried to imitate it with this goofy code
        public string pressOperator(char op)
        {
            //can't perform the operation if there isn't at least 1 value on the stack to add
            //if there isn't, do nothing
            if ( nStack.Count() ==  0 ) { return DisplayString; }
            switch (op)
            {
                case '+':
                    DisplayString = (double.Parse(DisplayString) + nStack.Pop()).ToString();
                    break;
                case '-':
                    DisplayString = (double.Parse(DisplayString) - nStack.Pop()).ToString();
                    break;
                case '*':
                    DisplayString = (double.Parse(DisplayString) * nStack.Pop()).ToString();
                    break;
                case '/':
                    DisplayString = (double.Parse(DisplayString) / nStack.Pop()).ToString();
                    break;
            }

            //setting enterPressed, so the next type on the calculator changes the DisplayString.
            enterPressed = true;
            return DisplayString;
        }
    }

    internal class NumStack
    {
        private Stack<double> stack;
        public NumStack()
        {
            stack = new Stack<double>();
        }

        public void Push(string numString)
        {
            stack.Push(double.Parse(numString));
        }

        public double Pop()
        {
            return stack.Pop();
        }

        public int Count()
        {
            return stack.Count;
        }
    }
}
