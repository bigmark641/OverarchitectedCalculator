using System;
using Calculator;

namespace TextCalculator
{
    interface IOperationFactory 
    {
        IOperation GetOperation(string operatorSymbol);
    }
}