using System;
using System.Reflection;
//using Android.Telecom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
//using Java.Util;

namespace RPNCalculator.Mobile;

public partial class MainPage : ContentPage
{

    //new class to store the values of the current history
    internal class CalcStatus
    {
        public Stack<string> calcStack;
        public int enterPressed;

        public CalcStatus(NumStack nStack, int nEPressed)
        {
            calcStack = new Stack<string>(nStack.stacks.Reverse());
            enterPressed = nEPressed;
        }


    }
    internal class CalcHistory
    {
        List<CalcStatus> History;
        int historySize = 10;

        public CalcHistory()
        {
            History = new List<CalcStatus>();
        }

        public void updateHistory(CalcStatus x)
        {
            History.Add(x);
            if (History.Count > historySize)
            {
                History.RemoveAt(0);
            }
        }

        public CalcStatus? getHistory()
        {
            //return the last object pushed onto the stack and remove it
            if (History.Count > 0)
            {
                CalcStatus returnVal = History[History.Count - 1];
                History.RemoveAt(History.Count - 1);
                return returnVal;
            }
            else
            {
                return null;
            }
        }

    }
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
        public Calculator()
        {
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
            if (nStack.Count() > 0 && ((nStack.Peek()[nStack.Peek().Length - 1] == '.') || numFloats == 0 && nStack.Peek().Contains('.'))) { DisplayString += "."; }
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
                    nStack.stacks = history.calcStack;
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
            double? returnValue = udf.CreateFunction(function, ref nStack.stacks);
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

    private bool isShift = false;
    private string inputStack = "";
    private string userInput = "";
    private int stackCounter = 1;
    int enterPressed;
    private Calculator calculator;
    private CalcHistory hist;

    public MainPage()
    {
        InitializeComponent();
        udf = new UserDefinedFunctions();
        nStack = new NumStack();
        calculator = new Calculator();
        hist = new CalcHistory();
    }
    internal class UserDefinedFunctions
    {
        public double? CreateFunction(string function, ref Stack<string> stack)
        {
            string baseFunction = @"
                
                using System;
                using System.Collections.Generic;

                namespace UserDefinedFunctions
                {
                    public class UDF
                    {
                        replace
                    }
                }";

            string completeFunction = baseFunction.Replace("replace", function);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(completeFunction);
            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Collections.Generic.Stack<string>).Assembly.Location),
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> diagnostics = result.Diagnostics;
                    return null;
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    Type type = assembly.GetType("UserDefinedFunctions.UDF");
                    object obj = Activator.CreateInstance(type);
                    object returnVal = type.InvokeMember("entryFunction",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { stack });
                    return (double)returnVal;
                }


            }
        }
    }
    internal class NumStack
    {
        public Stack<string> stacks;
        public NumStack()
        {
            stacks = new Stack<string>();
            stacks.Push("");
        }

        public void Push(string numString)
        {
            stacks.Push(numString);
        }

        public string Pop()
        {
            return stacks.Pop();
        }

        public int Count()
        {
            return stacks.Count;
        }

        public void updateTop(string update, int overwrite)
        {

            if (stacks.Count > 0)
            {
                if (overwrite == 0)
                {
                    stacks.Push(stacks.Pop() + update);
                }
                else if (overwrite == 1)
                {
                    stacks.Push(update);
                }
                else if (overwrite == 2)
                {
                    stacks.Pop();
                    stacks.Push(update);
                }
            }
            else
            {
                stacks.Push(update);
            }
        }

        // Method to get the stack items as a list of strings
        public List<string> GetStackItems()
        {
            // Convert the stack items to a list of strings
            List<string> stackList = new List<string>(stacks);
            stackList.Reverse(); // Reverse the list to maintain stack order
            return stackList;
        }

        public string Peek()
        {
            return stacks.Peek();
        }
    }
    private UserDefinedFunctions udf;
    private NumStack nStack;


    public void userDefinedFunction(string function)
    {
        double? returnValue = udf.CreateFunction(function, ref nStack.stacks);
        enterPressed = 1;
    }
    private void CompileandRunButton_Clciked(object sender, EventArgs e)
    {

    }
    private void UpdateDisplay()
    {
        // Example: Update a label to show the result
        result.Text = calculator.DisplayString;
    }

    void ShiftButtonClick(object sender, EventArgs e)
    {
        isShift = !isShift;
        if (isShift)
        {
            PiButton.Text = "x!";
            EulerButton.Text = "LN";
            LogButton.Text = "√x";
            TangentButton.Text = "arctan()";
            TangentButton.FontSize = 18;
            cosineButton.Text = "arccos()";
            cosineButton.FontSize = 18;
            sineButton.Text = "arcsin()";
            sineButton.FontSize = 18;
            xybutton.Text = "y√x";
        }
        else
        {
            PiButton.Text = "π";
            EulerButton.Text = "eⁿ";
            LogButton.Text = "LOG";
            TangentButton.Text = "tan()";
            TangentButton.FontSize = 27;
            cosineButton.Text = "cos()";
            cosineButton.FontSize = 27;
            sineButton.Text = "sin()";
            sineButton.FontSize = 27;
            xybutton.Text = "x^y";
        }

    }
    void Toggle(object sender, EventArgs e)
    {
        calculatorGrid.IsVisible = !calculatorGrid.IsVisible;
        functionDisplay.IsVisible = !functionDisplay.IsVisible;

        var button = sender as Button;
        button.Text = calculatorGrid.IsVisible ? "Toggle" : "Toggle";
    }

    private void NumberButton_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            calculator.pressNumber(Convert.ToChar(button.Text)); // add number to calculator
            result.Text = calculator.DisplayString;
        }
        string number = button.Text;

        // Check if the input stack length exceeds 10 characters
        if (inputStack.Length >= 10)
        {
            // If it exceeds, remove the first character from the input stack
            inputStack = inputStack.Substring(1);
        }

        // Append the entered number to the input stack
        inputStack = stackCounter.ToString() + ": " + number;

        // Update the stack Label with the input stack
        stack.Text = inputStack;

        // Append the entered number to the user input
        userInput += number;

        // Update the input display with the user input
        result.Text = userInput;
        var stackItems = calculator.GetTopStackItems();

        //stackCounter++;
    }

    private void EnterButton_Clicked(object sender, EventArgs e)
    {
        calculator.pressEnter();
        result.Text = calculator.DisplayString;

        if (!string.IsNullOrEmpty(userInput))
        {
            // Append the user input to the stack with a newline character
            inputStack += userInput + "\n";

            // Update the stack Label
            //stack.Text = inputStack;

            // Clear the user input for the next entry
            userInput = "";

        }
        var stackItems = calculator.GetTopStackItems(); // Limit to at most three items

    }
    private void OperatorButton_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            hist.updateHistory(new CalcStatus(nStack, enterPressed));

            calculator.pressOperator(button.Text); // add number to calculator
            result.Text = calculator.DisplayString;
            // Get the stack items and update the stack display
            var stackItems = calculator.GetTopStackItems();
            inputStack = string.Join("\n", stackItems) + "\n";
            StringBuilder numberedStack = new StringBuilder();
            int index = 1;
            foreach (var item in stackItems)
            {
                numberedStack.AppendLine($"{index++}: {item}");
            }

            // Update the stack Label with the numbered stack display
            stack.Text = numberedStack.ToString();
        }
    }
    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        calculator.pressClear();
        result.Text = calculator.DisplayString;
        // Clear the input stack
        inputStack = "";
        // Update the stack Label
        stack.Text = inputStack;
        stackCounter = 1;
        userInput = "";
        result.Text = "";

    }
}
