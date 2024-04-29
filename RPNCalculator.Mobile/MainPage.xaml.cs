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
/*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
namespace RPNCalculator.Mobile;

public partial class MainPage : ContentPage
{
    private bool isShift = false;

    // This will create and instantialize the calculator to perform calculations
    private Calculator calculator = new Calculator();

    public MainPage()
    {
        InitializeComponent();
    }

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This method will update the display with stack values
    private void UpdateDisplay()
    {
        // This will get the value of the stack and display it
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

            display += stackValuesList[i] + "\n"; // This will append the item followed by a newline
        }

        if (display.EndsWith("\n")) // This is added to trail the whitespace in order to make it look nicer
        {
            display = display.TrimEnd('\n');
        }

        stack.Text = display.TrimEnd();  // Thiss will set the built string to the DisplayText property, trimming any trailing newline
        result.Text = calculator.DisplayString;
    }

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This eventhandler will handle the actions of the shift button of the calculator
    void ShiftButtonClick(object sender, EventArgs e)
    {
        isShift = !isShift;
        if (isShift)
        {
            PiButton.Text = "x!";
            EulerButton.Text = "LN";
            LogButton.Text = "√x";
            TangentButton.Text = "arctan()";
            cosineButton.Text = "arccos()";
            sineButton.Text = "arcsin()";
            xybutton.Text = "y√x";
        }
        else
        {
            PiButton.Text = "π";
            EulerButton.Text = "eⁿ";
            LogButton.Text = "LOG";
            TangentButton.Text = "tan()";
            cosineButton.Text = "cos()";
            sineButton.Text = "sin()";
            xybutton.Text = "x^y";
        }

    }

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This eventhandler will handle switching between the calculator and the user defined function page when the toggle button is clicked
    void Toggle(object sender, EventArgs e)
    {
        CalculatorBorder.IsVisible = !CalculatorBorder.IsVisible;
        FunctionBorder.IsVisible = !FunctionBorder.IsVisible;

        var button = sender as Button;
        button.Text = CalculatorBorder.IsVisible ? "Toggle" : "Toggle";
    }

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This will handle displaying any numbers the user might click on onto the result screen
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

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This will handle displaying the results of the users calculations on the result screen when the user presses enter
    private void EnterButton_Clicked(object sender, EventArgs e)
    {
        calculator.pressEnter();
        UpdateDisplay();
    }

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This will handle displaying and calculating any calculations the user might make using any of the operations from the calculator
    private void OperatorButton_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            calculator.pressOperator(button.Text);
        }
        UpdateDisplay();
    }

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This will handle clearing the calculator screen when the clear button is clicked
    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        calculator.pressClear();
        UpdateDisplay();
    }

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This will handle decreasing the float of a number on the calculator screen
    private void DecreaseFloatButton_Click(object sender, EventArgs e)
    {
        calculator.pressOperator("addFloat");
        UpdateDisplay();
    }

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This will handle increasing the float of a number on the calculator screen
    private void IncreaseFloatButton_Click(object sender, EventArgs e)
    {
        calculator.pressOperator("removeFloat");
        UpdateDisplay();
    }

    /*
    Author: Saim Ishtiaq
    Description: Designing the mobile version of the RPNCalculator by solely working on the frontend and some of the backend
    Goal: Set up and design the frontend of the Mobile using .Maui and an android emulator
    Certification: I certify that I wrote all of the code in this file myself.
*/
    // This will handle compiling and running the function that the user enters in the user defined function section
    private void CompileButton_Click(object sender, EventArgs e)
    {
        calculator.userDefinedFunction(functionEditor.Text);
        functionEditor.Text = "";
        UpdateDisplay();
    }
}
