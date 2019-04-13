using System;
using System.Collections.Generic;

namespace Calculator.CalculatorEngine
{
    public interface IOperation
    {
        int NumberOfOperands();
        decimal ResultForOperands(IEnumerable<decimal> operands);
    }
}