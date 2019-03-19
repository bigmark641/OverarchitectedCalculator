using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Implementations.Operations
{
    [Operator("^2")]
    class SquareOperation : Operation
    {
        public override int GetNumberOfOperands()
            => 1;

        protected override decimal GetResultForValidatedOperands(IList<decimal> operands)
            => operands.Single() * operands.Single();
    }
}