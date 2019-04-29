using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;
using Calculator.Utilities;

namespace Calculator.OperationPlugins
{
    [Operator("PI")]
    public class PiOperation : Operation
    {
        public override int NumberOfOperands()
            => 0;

        protected override Validated<decimal> ResultForValidatedOperands(IList<decimal> operands)
            => new Validated<decimal>((decimal)Math.PI);
    }
}