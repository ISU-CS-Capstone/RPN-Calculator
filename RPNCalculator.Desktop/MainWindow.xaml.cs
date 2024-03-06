using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RPN_Calculator.Common;

namespace RPNCalculator.Desktop
{
    
    // inherit from INotifyPropertyChanged to bind text output to other class
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Calculator calculator = new Calculator();

        public event PropertyChangedEventHandler PropertyChanged; // handles page changes

        public string DisplayText
        {
            get { return calculator.getDisplayString(); }
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
                calculator.pressOperator(Convert.ToChar(button.Content)); // add operator to calc
                UpdateDisplay();
            }
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressEnter();
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
            if (e.Key >= Key.D0 && e.Key <= Key.D9) // Numbers
            {
                calculator.pressNumber((char)('0' + e.Key - Key.D0));
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) // Numpad Numbers
            {
                calculator.pressNumber((char)('0' + e.Key - Key.NumPad0));
            }
            else switch (e.Key) // Operators and Enter
            {
                case Key.Add:
                    calculator.pressOperator('+');
                    break;
                case Key.Subtract:
                    calculator.pressOperator('-');
                    break;
                case Key.Multiply:
                    calculator.pressOperator('*');
                    break;
                case Key.Divide:
                    calculator.pressOperator('/');
                    break;
                case Key.Enter:
                    calculator.pressEnter();
                    break;
                // Handle other keys or operators as needed
            }
            UpdateDisplay();
        }
    }
}