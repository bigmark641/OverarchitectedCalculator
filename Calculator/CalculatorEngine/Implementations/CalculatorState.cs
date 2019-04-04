using System;
using System.Collections.Immutable;
using CalculatorEngine;

namespace CalculatorEngine.Implementations
{
    public class CalculatorState : ICalculatorState
    {
        public IImmutableList<decimal> Values {get;}
        public IOperation ActiveOperation {get;}

        public CalculatorState(IImmutableList<decimal> values, IOperation operation)
        {
            Values = values;
            ActiveOperation = operation;
        }
    }
}