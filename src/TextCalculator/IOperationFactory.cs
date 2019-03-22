using System;
using CalculatorEngine;

namespace TextCalculator
{
    interface IOperationFactory 
    {
        IOperation GetOperationByOperatorSymbol(string operatorSymbol);
    }
}