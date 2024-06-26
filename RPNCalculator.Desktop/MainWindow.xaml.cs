﻿using System.ComponentModel;
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

        /**
         *  Kolby Christiansen - certify code written myself
         *  to get string to be displayed on caculator
         */
        public string DisplayText
        {
            get { return calculator.DisplayString; }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Kolby Christiansen - certify that this code was written by myself
        // Purpose - change text box to be new content as the class updates the display string along with updating stack values
        private void UpdateDisplay()
        {
            // Get the value of the stack and display it
            IEnumerable<string>? stackValues = calculator.GetTopStackItems();

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

        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose - event listener for when a calc digit is clicked
         */
        private void DigitButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                calculator.pressNumber(Convert.ToChar(button.Content)); // add number to calculator
                UpdateDisplay();
            }
        }
        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose - event listener for operator click
         */
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
        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose - event listener for pressing enter to push value onto stack
         */
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressEnter();
            UpdateDisplay();
        }
        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose - event listener to clear out whole calculator
         */
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressClear();
            UpdateDisplay();
        }
        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose - redirect to documentation page
         */
        private void DocumentationButton_Click(object sender, RoutedEventArgs e)
        {
            var docsPage = new DocumentationWindow();
            docsPage.Show();
            this.Close();
        }
        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose - compile listener to compile and run input code
         */
        private void CompileButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.userDefinedFunction(FunctionTextbox.Text);
            FunctionTextbox.Text = "";
            UpdateDisplay();
        }

        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose - listener to show less numbers after decimal in display
         */
        private void DecreaseFloatButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressOperator("addFloat");
            UpdateDisplay();
        }
        
        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose - listener to show more numbers after decimal in display
         */
        private void IncreaseFloatButton_Click(object sender, RoutedEventArgs e)
        {
            calculator.pressOperator("removeFloat");
            UpdateDisplay();
        }

        
        // Kolby Christiansen - certify that this code was written by myself
        // Purpose -  - Handle input of operators, numpad, enter, etc... for keyboard functionality
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
        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose -  - change to user functions
         */
        private void ToggleViewButton_Checked(object sender, RoutedEventArgs e)
        {
            CalcButtonBorder.Visibility = Visibility.Collapsed;  // Hide the calculator
            UserFunctionBorder.Visibility = Visibility.Visible;     // Show the textbox
        }
        /**
         * Kolby Christiansen - certify that this code was written by myself
         * Purpose - change to calc view
         */
        private void ToggleViewButton_Unchecked(object sender, RoutedEventArgs e)
        {
            CalcButtonBorder.Visibility = Visibility.Visible;   // Show the calculator
            UserFunctionBorder.Visibility = Visibility.Collapsed;  // Hide the textbox
        }        
    }
}