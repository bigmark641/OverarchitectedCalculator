using System;
using System.Collections.Generic;
using CalculatorEngine;

namespace OperationPlugins
{
    [Operator("+")]
    class AdditionOperation : Operation
    { 
        public override int GetNumberOfOperands()
            => 2;

        protected override decimal GetResultForValidatedOperands(IList<decimal> operands)
            => operands[0] + operands[1];
    }
}