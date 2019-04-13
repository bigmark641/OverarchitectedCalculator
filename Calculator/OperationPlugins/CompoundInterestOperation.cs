using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;

namespace Calculator.OperationPlugins
{
    [Operator("CompoundInterest")]
    public class CompoundInterestOperation : Operation
    {
        public override int NumberOfOperands()
            => 3;

        protected override decimal ResultForValidatedOperands(IList<decimal> operands)
        {
            return (decimal)compoundInterestResult((double)operands[0], (double)operands[1], (double)operands[2]);

            //Local functions
            double compoundInterestResult(double principle, double interestRate, double duration)
                => principle * Math.Pow(1 + interestRate, duration);
        }
    }
}