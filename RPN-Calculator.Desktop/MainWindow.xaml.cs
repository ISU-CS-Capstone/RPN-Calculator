using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RPN_Calculator.Desktop
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stack<double> stack;
        public MainWindow()
        {
            InitializeComponent();
            stack = new Stack<double>();
        }
        private void DigitButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string digit = button.Content.ToString();
            Display.Text += digit;
           
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string operation = button.Content.ToString();

            if (stack.Count >= 2)
            {
                double operand2 = stack.Pop();
                double operand1 = stack.Pop();
                double result = 0;

                switch (operation)
                {
                    case "+":
                        result = operand1 + operand2;
                        break;
                    case "-":
                        result = operand1 - operand2;
                        break;
                    case "*":
                        result = operand1 * operand2;
                        break;
                    case "/":
                        if (operand2 != 0)
                            result = operand1 / operand2;
                        else
                            MessageBox.Show("Error: Division by zero");
                        break;
                    default:
                        MessageBox.Show("Invalid operation");
                        return;
                }

                stack.Push(result);
                Display.Text = result.ToString();
            }
            else
            {
                MessageBox.Show("Insufficient operands");
            }
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Display.Text))
            {
                double value = double.Parse(Display.Text);
                stack.Push(value);
                Display.Clear();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            stack.Clear();
            Display.Clear();
        }
        
        private void DocumentationButton_Click(object sender, RoutedEventArgs e)
        {
            var docsPage = new DocumentationWindow();
            docsPage.Show();
            this.Close();
        }
    }
}