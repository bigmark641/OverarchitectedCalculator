using System;
using Calculator;

namespace TextCalculator.Implementations
{
    class TextCalculator : ITextCalculator
    {
        private ICalculator Calculator {get;}
        private IOperationFactory OperationFactory {get;}

        public TextCalculator(ICalculator calculator, IOperationFactory operationFactory)
        {
            Calculator = calculator;
            OperationFactory = operationFactory;
        }

        public string SubmitInputAndGetResult(string input)
        {
            return decimalResult().ToString();
            decimal decimalResult() => IsNumber(input)
                ? Calculator.SubmitValueInputAndGetResult(numberInput())
                : Calculator.SubmitOperationInputAndGetResult(operationInput());
            decimal numberInput() => GetNumber(input);
            IOperation operationInput() => OperationFactory.GetOperation(input);
        }

        private bool IsNumber(string stringToTest)
            => decimal.TryParse(stringToTest, out var result);

        private decimal GetNumber(string stringToConvert)
            => decimal.Parse(stringToConvert);
    }
}