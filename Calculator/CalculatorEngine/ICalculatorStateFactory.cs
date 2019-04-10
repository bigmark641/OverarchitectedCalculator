using System;
using System.Collections.Immutable;

namespace Calculator.CalculatorEngine
{
    public interface ICalculatorStateFactory
    {
        ICalculatorState GetCalculatorState(IImmutableList<decimal> values, IOperation operation);
    }
}