using System;
using System.Collections.Immutable;
using Calculator.Implementations;

namespace Calculator.Implementations
{
    class CalculatorStateFactory : ICalculatorStateFactory
    {
        public ICalculatorState GetCalculatorState(IImmutableList<decimal> values, IOperation operation)
            => new CalculatorState(values, operation);
    }
}