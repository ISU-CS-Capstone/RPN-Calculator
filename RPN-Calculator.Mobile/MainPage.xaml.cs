namespace RPN_Calculator.Mobile;

public partial class MainPage : ContentPage
{

	private bool isShift = false;
	
    public MainPage()
	{
		InitializeComponent();
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

