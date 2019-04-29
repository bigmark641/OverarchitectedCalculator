using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.CalculatorEngine;
using Calculator.Utilities;

namespace Calculator.OperationPlugins
{
    [Operator("CompoundInterest")]
    public class CompoundInterestOperation : Operation
    {
        public override int NumberOfOperands()
            => 3;

        protected override Validated<decimal> ResultForValidatedOperands(IList<decimal> operands)
        {
            return compoundInterestResult((double)operands[0], (double)operands[1], (double)operands[2])
                .Map(x => (decimal) x);

            //Local functions
            Validated<double> compoundInterestResult(double principle, double interestRate, double duration)
                => new Validated<double>(principle * Math.Pow(1 + interestRate, duration));
        }
    }
}