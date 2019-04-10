using System;
using System.Collections.Generic;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    [Operator("+")]
    public class AdditionOperation : Operation
    { 
        public override int GetNumberOfOperands()
            => 2;

        protected override decimal GetResultForValidatedOperands(IList<decimal> operands)
            => operands[0] + operands[1];
    }
}