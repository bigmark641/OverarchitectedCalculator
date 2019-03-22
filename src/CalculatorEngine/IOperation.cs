using System;
using System.Collections.Generic;

namespace CalculatorEngine
{
    interface IOperation
    {
        int GetNumberOfOperands();
        decimal GetResultForOperands(IList<decimal> operands);
    }
}