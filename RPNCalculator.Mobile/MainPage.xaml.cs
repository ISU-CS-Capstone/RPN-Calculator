using RPNCalculator.Common;

namespace RPNCalculator.Mobile;

public partial class MainPage : ContentPage
{
    RPNCalculator.Common.Calculator calc;
    private bool isShift = false;

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
        //calc.pressEnter();
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
}
