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
            StackDisplay.Text = display.TrimEnd();  // Set the built string to the DisplayText property, trimming any trailing newline

            // Notify UI that DisplayText has changed
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
            var button = (Button)sender;
            string? cmd = button.Tag.ToString();
            if (cmd == null)
            {
                Console.WriteLine("Invalid tag for input button: " + button.Tag);
                return;
            }
            calculator.pressOperator(cmd); // add operator to calc
            UpdateDisplay();
        }
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressEnter();
            UpdateDisplay();
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(((Button)sender).Tag);
            calculator.pressClear();
            UpdateDisplay();
        }
        private void DocumentationButton_Click(object sender, RoutedEventArgs e)
        {
            var docsPage = new DocumentationWindow();
            docsPage.Show();
            this.Close();
        }

        private void CompileButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.userDefinedFunction(FunctionTextbox.Text);
            FunctionTextbox.Text = "";
            UpdateDisplay();
        }

        private void DecreaseFloatButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressOperator("addFloat");
            UpdateDisplay();
        }

        private void IncreaseFloatButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressOperator("removeFloat");
            UpdateDisplay();
        }

        
        // Handle input of operators, numpad, enter, etc...
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (CalcButtonBorder.Visibility == Visibility.Collapsed) return; // don't capture input's if typing into user defined textbox
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
        private void ToggleViewButton_Checked(object sender, RoutedEventArgs e)
        {
            CalcButtonBorder.Visibility = Visibility.Collapsed;  // Hide the calculator
            UserFunctionBorder.Visibility = Visibility.Visible;     // Show the textbox
        }

        private void ToggleViewButton_Unchecked(object sender, RoutedEventArgs e)
        {
            CalcButtonBorder.Visibility = Visibility.Visible;   // Show the calculator
            UserFunctionBorder.Visibility = Visibility.Collapsed;  // Hide the textbox
        }        
    }
}