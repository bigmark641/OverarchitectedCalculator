using System;
using System.Collections.Generic;
using Calculator.CalculatorEngine;
using Calculator.Utilities;

namespace Calculator.OperationPlugins
{
    [Operator("-")]
    public class SubtractionOperation : Operation
    {
        public override int NumberOfOperands()
            => 2;

        protected override Validated<decimal> ResultForValidatedOperands(IList<decimal> operands)
            => new Validated<decimal>(operands[0] - operands[1]);
    }
}