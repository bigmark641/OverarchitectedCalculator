using System;

namespace Calculator.Implementations.Operations
{
    [Operator("neg")]
    class NegationOperation : UnaryOperation
    {
        protected override decimal GetResultForOneOperand(decimal operand)
        {
            return -operand;
        } 
    }
}