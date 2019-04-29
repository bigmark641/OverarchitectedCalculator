using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;
using Calculator.Utilities;

namespace Calculator.OperationPlugins
{
    [Operator("sqrt")]
    public class SquareRootOperation : Operation
    {
        public override int NumberOfOperands()
            => 1;

        protected override Validated<decimal> ResultForValidatedOperands(IList<decimal> operands)
        {
            return isOperandValid()
                ? new Validated<decimal>((decimal) System.Math.Sqrt(operandAsDouble()))
                : new StringError("Only positive numbers can be square rooted.");

            //Local functions
            bool isOperandValid()
                => !double.IsNaN(System.Math.Sqrt(operandAsDouble()));
            double operandAsDouble()
                => (double) operands.Single();
        }
    }
}