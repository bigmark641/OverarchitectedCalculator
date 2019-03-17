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
            decimal decimalResult = IsNumber(input)
                ? Calculator.SubmitValueInputAndGetResult(GetNumber(input))
                : Calculator.SubmitOperationInputAndGetResult(OperationFactory.CreateOperation(input));
            return decimalResult.ToString();
        }

        private bool IsNumber(string stringToTest)
        {
            return decimal.TryParse(stringToTest, out var result);
        }

        private decimal GetNumber(string stringToConvert)
        {
            return decimal.Parse(stringToConvert);
        }
    }
}