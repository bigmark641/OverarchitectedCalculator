using System;

namespace Calculator.Implementations.Operations
{
    [Operator("sqrt")]
    class SquareRootOperation : UnaryOperation
    {
        protected override decimal GetResultForOneOperand(decimal operand)
        {
            return (decimal) System.Math.Sqrt((double) operand);
        } 
    }
}