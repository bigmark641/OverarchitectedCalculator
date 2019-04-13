using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    [Operator("PI")]
    public class PiOperation : Operation
    {
        public override int NumberOfOperands()
            => 0;

        protected override decimal ResultForValidatedOperands(IList<decimal> operands)
            => (decimal)Math.PI;
    }
}