using System;
using Calculator.Utilities;

namespace Calculator.CalculatorEngine
{
    public interface ICalculator
    {
        Validated<decimal> SubmitValueInputAndGetResult(decimal value);
        Validated<decimal> SubmitOperationInputAndGetResult(IOperation operation);
        Validated<decimal> SubmitEqualsRequestAndGetResult();
    }
}