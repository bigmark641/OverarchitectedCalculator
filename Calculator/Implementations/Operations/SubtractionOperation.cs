using System;

namespace Calculator.Implementations.Operations
{
    [Operator("-")]
    class SubtractionOperation : BinaryOperation
    {
        protected override decimal GetResultForTwoOperands(decimal operand1, decimal operand2)
        {
            return operand1 - operand2;
        }
    }
}