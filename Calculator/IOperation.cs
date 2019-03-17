using System;

namespace Calculator
{
    interface IOperation
    {
        int GetNumberOfOperands();
        decimal GetResultForOperands(params decimal[] operands);
    }
}