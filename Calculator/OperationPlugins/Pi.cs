using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    [Operator("PI")]
    class PiOperation : Operation
    {
        public override int GetNumberOfOperands()
            => 0;

        protected override decimal GetResultForValidatedOperands(IList<decimal> operands)
            => (decimal)Math.PI;
    }
}