using System;

namespace CalculatorEngine
{
    interface ICalculator
    {
        decimal SubmitValueInputAndGetResult(decimal value);
        decimal SubmitOperationInputAndGetResult(IOperation operation);
    }
}