using System;
using TextCalculator;

namespace Calculator
{
    class Program
    {
        static readonly ITextCalculator TextCalculator = CreateTextCalculator();

        static void Main(string[] args)
        {
            //Simulate user input
            SubmitUserInputAndPrintResultWithDelay("5");
            SubmitUserInputAndPrintResultWithDelay("+");
            SubmitUserInputAndPrintResultWithDelay("2");
            SubmitUserInputAndPrintResultWithDelay("-");
            SubmitUserInputAndPrintResultWithDelay("3");
            SubmitUserInputAndPrintResultWithDelay("neg");
            SubmitUserInputAndPrintResultWithDelay("^2");
            SubmitUserInputAndPrintResultWithDelay("sqrt");
            SubmitUserInputAndPrintResultWithDelay("CompoundInterest");
            SubmitUserInputAndPrintResultWithDelay(".1");
            SubmitUserInputAndPrintResultWithDelay("2");
        }

        private static ITextCalculator CreateTextCalculator()
        {
            //Construct ITextCalculator with injected dependencies
            var calculatorStateFactory = new Calculator.Implementations.CalculatorStateFactory();
            var calculator = new Calculator.Implementations.Calculator(calculatorStateFactory);
            var operationFactory = new OperationFactory();
            return new TextCalculator.Implementations.TextCalculator(calculator, operationFactory);
        }

        private static void SubmitUserInputAndPrintResultWithDelay(string input)
        {
            //Print input
            PrintInput(input);
            Delay();     
            //Submit
            var result = TextCalculator.SubmitInputAndGetResult(input);
            //Print result
            PrintResult(result);
            Delay();
        }

        private static void PrintInput(string input)
        {
            const string INPUT_FORMATTER = "Submitting input: {0}";
            var stringToPrint = string.Format(INPUT_FORMATTER, input);
            Console.WriteLine(stringToPrint);
        }

        private static void PrintResult(string result)
        {
            const string RESULT_FORMATTER = "              Receiving result: {0}";
            var stringToPrint = string.Format(RESULT_FORMATTER, result);
            Console.WriteLine(stringToPrint);
        }

        private static void Delay()
        {
            const int MILLISECONDS_TO_DELAY = 500;
            System.Threading.Thread.Sleep(MILLISECONDS_TO_DELAY);
        }
    }
}
