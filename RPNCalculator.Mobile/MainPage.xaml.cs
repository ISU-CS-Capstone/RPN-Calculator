using System;
using System.Reflection;
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
using RPNCalculator.Common;

namespace RPNCalculator.Mobile;

public partial class MainPage : ContentPage
{
    private bool isShift = false;
    private string inputStack = "";
    private string userInput = "";
    //private int stackCounter = 1;
    
    private Calculator calculator = new Calculator();

    public MainPage()
    {
        InitializeComponent();
    }
    
    private void UpdateDisplay()
    {
        // Get the value of the stack and display it
        IEnumerable<string> stackValues = calculator.GetTopStackItems();

        // Convert IEnumerable to List to access by index
        List<string> stackValuesList = stackValues.ToList();

        // Initialize display with 5 newlines
        string display = "\n\n\n\n\n";

        // Limit the display to the top 5 stack items, if more than 5
        for (int i = 0; i < stackValuesList.Count && i < 5; i++)
        {
            // Adjust the number of leading newlines based on the number of items
            if (i > 0)
            {
                display = display.Substring(1); // Remove one newline character for each additional item
            }

            display += stackValuesList[i] + "\n"; // Append the item followed by a newline
        }

        if (display.EndsWith("\n")) // trail whitespace to look nicer
        {
            display = display.TrimEnd('\n');
        }

        // Assuming there is a property called DisplayText for data binding
        stack.Text = display.TrimEnd();  // Set the built string to the DisplayText property, trimming any trailing newline
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
        UpdateDisplay();
    }

    private void EnterButton_Clicked(object sender, EventArgs e)
    {
        calculator.pressEnter();
        UpdateDisplay();
    }
    private void OperatorButton_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            calculator.pressOperator(button.Text);
        }
        UpdateDisplay();
    }
    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        calculator.pressClear();
        UpdateDisplay();
    }
}
