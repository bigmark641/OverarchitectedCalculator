using System;
using System.Collections.Generic;
using System.Linq;
using CalculatorEngine;

namespace OperationPlugins
{
    [Operator("CompoundInterest")]
    class CompoundInterestOperation : Operation
    {
        public override int GetNumberOfOperands()
            => 3;

        protected override decimal GetResultForValidatedOperands(IList<decimal> operands)
        {
            return (decimal)compoundedTotal((double)operands[0], (double)operands[1], (double)operands[2]);
            double compoundedTotal(double principle, double interestRate, double duration)
                => principle * Math.Pow(1 + interestRate, duration);
        }
    }
}