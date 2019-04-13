using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    [Operator("sqrt")]
    public class SquareRootOperation : Operation
    {
        public override int NumberOfOperands()
            => 1;

        protected override decimal ResultForValidatedOperands(IList<decimal> operands)
        {
            return isOperandValid() ?
                (decimal) System.Math.Sqrt(operandAsDouble()) :
                throw new System.ArgumentOutOfRangeException();

            //Local functions
            bool isOperandValid()
                => !double.IsNaN(System.Math.Sqrt(operandAsDouble()));
            double operandAsDouble()
                => (double) operands.Single();
        }
    }
}