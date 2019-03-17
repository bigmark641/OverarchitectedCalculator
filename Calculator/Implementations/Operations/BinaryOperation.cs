using System;
using System.Linq;
using Calculator;

namespace Calculator.Implementations.Operations
{  
    abstract class BinaryOperation : IOperation
    {

        protected abstract decimal GetResultForTwoOperands(decimal operand1, decimal operand2);

        public decimal GetResultForOperands(params decimal[] operands)
        {
            return operands.Count() == 2
                ? GetResultForTwoOperands(operands[0], operands[1])
                : throw new ArgumentException();
        }

        public int GetNumberOfOperands()
        {
            return 2;
        }
    }
}