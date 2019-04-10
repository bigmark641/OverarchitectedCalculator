using System;
using System.Collections.Generic;

namespace Calculator.CalculatorEngine
{
    public interface IOperation
    {
        int GetNumberOfOperands();
        decimal GetResultForOperands(IEnumerable<decimal> operands);
    }
}