using System;
using Calculator.CalculatorEngine;
using Calculator.Utilities;

namespace Calculator.TextCalculator.Implementations
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
            return validatedResult()
                .Match(
                    ifValue: x => x.ToString(),
                    ifError: x => "Error: " + x.ToString());

            //Local functions 
            Validated<decimal> validatedResult() => 
                IsNumber(input) 
                    ? Calculator.SubmitValueInputAndGetResult(numberInput())
                : input.Equals("=")
                    ? Calculator.SubmitEqualsRequestAndGetResult()
                    : Calculator.SubmitOperationInputAndGetResult(operationInput());            
            decimal numberInput() => AsNumber(input);
            IOperation operationInput() => OperationFactory.GetOperationByOperatorSymbol(input);
        }

        private bool IsNumber(string stringToTest)
            => decimal.TryParse(stringToTest, out var result);

        private decimal AsNumber(string stringToConvert)
            => decimal.Parse(stringToConvert);
    }
}