using System;
using Calculator.CalculatorEngine;

namespace Calculator.TextCalculator
{
    public interface IOperationFactory 
    {
        IOperation GetOperationByOperatorSymbol(string operatorSymbol);
    }
}