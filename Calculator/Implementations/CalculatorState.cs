using System;
using System.Collections.Immutable;
using Calculator;

namespace Calculator.Implementations
{
    class CalculatorState : ICalculatorState
    {
        public IImmutableList<decimal> Values {get;}
        public IOperation Operation {get;}

        public CalculatorState(IImmutableList<decimal> values, IOperation operation)
        {
            Values = values;
            Operation = operation;
        }
    }
}