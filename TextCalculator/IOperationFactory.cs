using System;
using Calculator;

namespace TextCalculator
{
    interface IOperationFactory 
    {
        IOperation CreateOperation(string operatorSymbol);
    }
}