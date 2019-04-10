using System;
using System.Collections.Immutable;
using Calculator.CalculatorEngine.Implementations;

namespace Calculator.CalculatorEngine.Implementations
{
    public class CalculatorStateFactory : ICalculatorStateFactory
    {
        public ICalculatorState GetCalculatorState(IImmutableList<decimal> values, IOperation operation)
            => new CalculatorState(values, operation);
    }
}