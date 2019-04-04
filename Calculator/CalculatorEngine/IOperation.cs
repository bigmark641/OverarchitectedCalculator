using System;
using System.Collections.Generic;

namespace CalculatorEngine
{
    public interface IOperation
    {
        int GetNumberOfOperands();
        decimal GetResultForOperands(IEnumerable<decimal> operands);
    }
}