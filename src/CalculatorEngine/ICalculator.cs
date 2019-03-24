using System;

namespace CalculatorEngine
{
    public interface ICalculator
    {
        decimal SubmitValueInputAndGetResult(decimal value);
        decimal SubmitOperationInputAndGetResult(IOperation operation);
        decimal SubmitEqualsRequestAndGetResult();
    }
}