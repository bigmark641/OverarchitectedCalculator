using System;
using Calculator;

namespace Calculator
{
    interface IOperationFactory 
    {
        Func<string, IOperation> GetOperationByOperatorSymbol { get; }
    }
}