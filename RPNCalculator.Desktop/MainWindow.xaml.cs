using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RPNCalculator.Common;

namespace RPNCalculator.Desktop
{
    
    // inherit from INotifyPropertyChanged to bind text output to other class
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Calculator calculator = new Calculator();

        public event PropertyChangedEventHandler PropertyChanged; // handles page changes

        public string DisplayText
        {
            get { return calculator.DisplayString; }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        // change text box to be new content as the class updates the display string
        private void UpdateDisplay()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayText)));
        }
        private void DigitButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                calculator.pressNumber(Convert.ToChar(button.Content)); // add number to calculator
                UpdateDisplay();
            }
        }
        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                calculator.pressOperator(button.Content.ToString()); // add operator to calc
                UpdateDisplay();
            }
        }
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressEnter();
            UpdateDisplay();
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressClear();
            UpdateDisplay();
        }
        private void DocumentationButton_Click(object sender, RoutedEventArgs e)
        {
            var docsPage = new DocumentationWindow();
            docsPage.Show();
            this.Close();
        }

        
        // Handle input of operators, numpad, enter, etc...
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            bool shiftPressed = e.KeyboardDevice.IsKeyDown(Key.LeftShift) || e.KeyboardDevice.IsKeyDown(Key.RightShift); // capture shift input to allow operators on main keyboard base

            // Handle Numbers
            if (e.Key >= Key.D0 && e.Key <= Key.D9 && !shiftPressed) // Main keyboard numbers, no shift pressed
            {
                calculator.pressNumber((char)('0' + e.Key - Key.D0));
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) // Numpad numbers
            {
                calculator.pressNumber((char)('0' + e.Key - Key.NumPad0));
            }
            // Handle + and -
            else if (shiftPressed)
            {
                switch (e.Key)
                {
                    case Key.OemPlus:
                    case Key.Add:
                        calculator.pressOperator("+");
                        break;
                    case Key.OemMinus:
                    case Key.Subtract:
                        calculator.pressOperator("-");
                        break;
                }
            }
            else // handle * / and enter
            {
                switch (e.Key)
                {
                    case Key.Multiply:
                        calculator.pressOperator("*");
                        break;
                    case Key.Divide:
                    case Key.OemQuestion:
                        calculator.pressOperator("/");
                        break;
                    case Key.Enter:
                        calculator.pressEnter();
                        break;
                }
            }
            // Handling Shift + Key combinations
            if (shiftPressed)
            {
                switch (e.Key)
                {
                    case Key.D8: // shift + 8
                        calculator.pressOperator("*");
                        break;
                    case Key.OemPlus: // shift + =
                        calculator.pressOperator("+");
                        break;
                }
            }
            UpdateDisplay();
        }
    }
}