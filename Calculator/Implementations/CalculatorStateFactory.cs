using System;
using System.Collections.Immutable;
using Calculator.Implementations;

namespace Calculator.Implementations
{
    class CalculatorStateFactory : ICalculatorStateFactory
    {
        public ICalculatorState CreateCalculatorState(IImmutableList<decimal> values, IOperation operation)
        {
            return new CalculatorState(values, operation);
        }
    }
}