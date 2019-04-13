using System;
using System.Collections.Generic;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    [Operator("-")]
    public class SubtractionOperation : Operation
    {
        public override int NumberOfOperands()
            => 2;

        protected override decimal ResultForValidatedOperands(IList<decimal> operands)
            => operands[0] - operands[1];
    }
}