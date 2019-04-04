using System;
using System.Collections.Immutable;

namespace CalculatorEngine
{
    public interface ICalculatorStateFactory
    {
        ICalculatorState GetCalculatorState(IImmutableList<decimal> values, IOperation operation);
    }
}