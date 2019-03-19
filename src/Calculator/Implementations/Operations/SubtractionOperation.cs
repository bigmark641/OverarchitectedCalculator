using System;
using System.Collections.Generic;

namespace Calculator.Implementations.Operations
{
    [Operator("-")]
    class SubtractionOperation : Operation
    {
        public override int GetNumberOfOperands()
            => 2;

        protected override decimal GetResultForValidatedOperands(IList<decimal> operands)
            => operands[0] - operands[1];
    }
}