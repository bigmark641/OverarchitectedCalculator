using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;
using Calculator.Utilities;

namespace Calculator.OperationPlugins
{
    public abstract class Operation : IOperation
    {
        public abstract int NumberOfOperands();

        public Validated<decimal> ResultForOperands(IEnumerable<decimal> operands)
            => operands.Count() == NumberOfOperands()
                ? ResultForValidatedOperands(operands.ToList())
                : throw new ArgumentException();

        protected abstract Validated<decimal> ResultForValidatedOperands(IList<decimal> operands);
    }
}