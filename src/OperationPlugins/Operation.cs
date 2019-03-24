using System;
using System.Collections.Generic;
using System.Linq;
using CalculatorEngine;

namespace OperationPlugins
{
    abstract class Operation : IOperation
    {
        public abstract int GetNumberOfOperands();

        public decimal GetResultForOperands(IEnumerable<decimal> operands)
            => operands.Count() == GetNumberOfOperands()
                ? GetResultForValidatedOperands(operands.ToList())
                : throw new ArgumentException();

        protected abstract decimal GetResultForValidatedOperands(IList<decimal> operands);
    }
}