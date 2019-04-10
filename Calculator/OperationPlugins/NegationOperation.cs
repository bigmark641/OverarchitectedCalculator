using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    [Operator("neg")]
    class NegationOperation : Operation
    {
        public override int GetNumberOfOperands()
            => 1;

        protected override decimal GetResultForValidatedOperands(IList<decimal> operands)
            => -operands.Single();
    }
}