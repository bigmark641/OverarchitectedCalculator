using System;
using CalculatorEngine;

namespace TextCalculator
{
    public interface IOperationFactory 
    {
        IOperation GetOperationByOperatorSymbol(string operatorSymbol);
    }
}