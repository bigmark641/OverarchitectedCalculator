using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    public abstract class Operation : IOperation
    {
        public abstract int NumberOfOperands();

        public decimal ResultForOperands(IEnumerable<decimal> operands)
            => operands.Count() == NumberOfOperands()
                ? ResultForValidatedOperands(operands.ToList())
                : throw new ArgumentException();

        protected abstract decimal ResultForValidatedOperands(IList<decimal> operands);
    }
}