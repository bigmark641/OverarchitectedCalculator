using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Implementations.Operations
{
    [Operator("sqrt")]
    class SquareRootOperation : Operation
    {
        public override int GetNumberOfOperands()
            => 1;

        protected override decimal GetResultForValidatedOperands(IList<decimal> operands)
            => (decimal) System.Math.Sqrt((double) operands.Single());
    }
}