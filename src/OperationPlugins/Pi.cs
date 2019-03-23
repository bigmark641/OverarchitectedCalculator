using System;
using System.Collections.Generic;
using System.Linq;
using CalculatorEngine;

namespace OperationPlugins
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