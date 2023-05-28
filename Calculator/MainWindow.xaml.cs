using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double firstNumber = 0;
        private double secondNumber = 0;
        private string currentOperator = string.Empty;
        private bool isFirstOperator = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnDigitButtonClicked(object sender, RoutedEventArgs e)
        {
            if(sender is not Button button)
                return;
            if(button.Content is not string digitString)
                return;
            var isNumber = int.TryParse(digitString, out var digit);
            if (isNumber && digit is >= 0 and <= 9)
            {
                Display.Text += digitString;
            }
        }

        private void OnDeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length == 0)
            {
                firstNumber=0;
                secondNumber = 0;
                return;
            }
            Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
        }

        private void OnOperatorButtonClicked(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Content is not string operatorString)
                return;

            if (!string.IsNullOrEmpty(Display.Text))
                double.TryParse(ExtractNumberFromDisplayText(), out secondNumber);

            if (isFirstOperator)
            {
                firstNumber = secondNumber;
                isFirstOperator = false;
            }
            else
            {
                firstNumber = Calculate();
                Display.Text = firstNumber.ToString();
            }

            if (operatorString != "=")
            {
                currentOperator = operatorString;
                if ((operatorString == "-" || operatorString == "x" || operatorString == "+" || operatorString == "/" || operatorString =="%") && Display.Text == "0")
                {
                    Display.Text = string.Empty;
                }
                else
                {
                    Display.Text += operatorString;
                }
            }
            else
            {
                firstNumber = 0;
                if (Display.Text[0] == '-')
                {
                    currentOperator = "-";
                }
                else
                {
                    currentOperator = "+";
                }
                isFirstOperator = true;
            }
        }
    
        private string ExtractNumberFromDisplayText()
        {
            string numberString = string.Empty;
            for (int i = Display.Text.Length - 1; i >= 0; i--)
            {
                char c = Display.Text[i];
                if (c == '+' || c == '-' || c == 'x' || c == '%')
                {
                    break;
                }
                numberString = c + numberString;
            }
            return numberString;
        }

        public double Calculate()
        {
            switch (currentOperator)
            {
                case "+":
                    return firstNumber + secondNumber;
                case "-":
                    return firstNumber - secondNumber;
                case "x":
                    return firstNumber * secondNumber;
                case "%":
                    return firstNumber % secondNumber;
                default:
                    return 0;
            }

        }
    }
}
