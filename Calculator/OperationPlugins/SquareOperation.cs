using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    [Operator("^2")]
    public class SquareOperation : Operation
    {
        public override int NumberOfOperands()
            => 1;

        protected override decimal ResultForValidatedOperands(IList<decimal> operands)
            => operands.Single() * operands.Single();
    }
}