using System;
using CalculatorEngine;

namespace TextCalculator
{
    interface IOperationFactory 
    {
        Func<string, IOperation> GetOperationByOperatorSymbol { get; }
    }
}