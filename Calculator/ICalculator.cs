using System;
using Calculator;

namespace Calculator
{
    interface ICalculator
    {
        decimal SubmitValueInputAndGetResult(decimal value);
        decimal SubmitOperationInputAndGetResult(IOperation operation);
    }
}