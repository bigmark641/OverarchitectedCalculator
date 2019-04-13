using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    [Operator("neg")]
    public class NegationOperation : Operation
    {
        public override int NumberOfOperands()
            => 1;

        protected override decimal ResultForValidatedOperands(IList<decimal> operands)
            => -operands.Single();
    }
}