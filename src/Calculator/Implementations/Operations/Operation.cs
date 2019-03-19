using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Implementations
{
    abstract class Operation : IOperation
    {
        public abstract int GetNumberOfOperands();

        public decimal GetResultForOperands(IList<decimal> operands)
            => operands.Count() == GetNumberOfOperands()
                ? GetResultForValidatedOperands(operands)
                : throw new ArgumentException();

        protected abstract decimal GetResultForValidatedOperands(IList<decimal> operands);
    }
}