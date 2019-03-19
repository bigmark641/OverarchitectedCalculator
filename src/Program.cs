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
            //Get ITextCalculator with injected dependencies
            return new TextCalculator.Implementations.TextCalculator(getCalculator(), getOperationFactory());
            ICalculator getCalculator() => new Calculator.Implementations.Calculator(getCalculatorStateFactory());
            ICalculatorStateFactory getCalculatorStateFactory() => new Calculator.Implementations.CalculatorStateFactory();
            IOperationFactory getOperationFactory() => new OperationFactory();
        }

        private static void SubmitUserInputAndPrintResultWithDelay(string input)
        {
            //Print input
            PrintInput(input);
            Delay();
            //Print result
            PrintResult(result());
            string result() => TextCalculator.SubmitInputAndGetResult(input);
            Delay();
        }

        private static void PrintInput(string input)
        {
            Console.WriteLine(stringToPrint());
            string stringToPrint() => $"Submitting input: {input}";
        }

        private static void PrintResult(string result)
        {
            Console.WriteLine(stringToPrint());
            string stringToPrint() => $"              Receiving result: {result}";
        }

        private static void Delay()
        {
            System.Threading.Thread.Sleep(MILLISECONDS_TO_DELAY());
            int MILLISECONDS_TO_DELAY() => 500;
        }
    }
}
