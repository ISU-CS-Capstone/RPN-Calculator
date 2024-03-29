﻿using RPNCalculator.Common;

namespace RPNCalculator.Mobile;

public partial class MainPage : ContentPage
{
    RPNCalculator.Common.Calculator calc;
    public MainPage()
    {
        InitializeComponent();
        calc = new RPNCalculator.Common.Calculator();
    }

    private async void OnDocumentationLinkClicked(object sender, EventArgs e)
    {
        // Navigate to the DocumentationPage
        await Shell.Current.GoToAsync("///DocumentationPage");
    }

    private void NumberButton_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            calc.pressNumber(Convert.ToChar(button.Text)); // add number to calculator
            result.Text = calc.DisplayString;
        }
    }

    private void EnterButton_Clicked(object sender, EventArgs e)
    {
        calc.pressEnter();
        result.Text = calc.DisplayString;
    }

    private void OperatorButton_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            calc.pressOperator(Convert.ToChar(button.Text)); // add number to calculator
            result.Text = calc.DisplayString;
        }
        
    }

    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        calc.pressClear();
        result.Text = calc.DisplayString;
    }

}
