using System;

namespace Calculator.Implementations.Operations
{
    [Operator("^2")]
    class SquareOperation : UnaryOperation
    {
        protected override decimal GetResultForOneOperand(decimal operand)
        {
            return operand * operand;
        } 
    }
}