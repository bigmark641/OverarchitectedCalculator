using System;
using System.Linq;
using Calculator;

namespace Calculator.Implementations.Operations
{  
    abstract class UnaryOperation : IOperation
    {

        protected abstract decimal GetResultForOneOperand(decimal operand);

        public decimal GetResultForOperands(params decimal[] operands)
        {
            return operands.Count() == 1
                ? GetResultForOneOperand(operands.First())
                : throw new ArgumentException();
        }

        public int GetNumberOfOperands()
        {
            return 1;
        }
    }
}