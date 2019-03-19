using System;
using System.Collections.Generic;

namespace Calculator
{
    interface IOperation
    {
        int GetNumberOfOperands();
        //TODO: upgrade array to IEnumerable
        decimal GetResultForOperands(IList<decimal> operands);
    }
}