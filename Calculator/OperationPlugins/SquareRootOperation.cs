using System;
using System.Collections.Generic;
using System.Linq;
using CalculatorEngine;

namespace OperationPlugins
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