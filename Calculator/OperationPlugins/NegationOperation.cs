using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;
using Calculator.Utilities;

namespace Calculator.OperationPlugins
{
    [Operator("neg")]
    public class NegationOperation : Operation
    {
        public override int NumberOfOperands()
            => 1;

        protected override Validated<decimal> ResultForValidatedOperands(IList<decimal> operands)
            => new Validated<decimal>(-operands.Single());
    }
}