using System;
using System.Linq;

namespace Calculator.Implementations.Operations
{
    [Operator("CompoundInterest")]
    class CompoundInterestOperation : IOperation
    {
        public int GetNumberOfOperands()
        {
            return 3;
        }

        public decimal GetResultForOperands(params decimal[] operands)
        {
            return operands.Count() == 3
                ? operands[0] * (decimal)Math.Pow(1 + (double)operands[1], (double)operands[2])
                : throw new ArgumentException();
        }
    }
}