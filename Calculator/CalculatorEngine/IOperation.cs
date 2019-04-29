using System;
using System.Collections.Generic;
using Calculator.Utilities;

namespace Calculator.CalculatorEngine
{
    public interface IOperation
    {
        int NumberOfOperands();
        Validated<decimal> ResultForOperands(IEnumerable<decimal> operands);
    }
}