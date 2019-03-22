using System;
using System.Collections.Immutable;

namespace CalculatorEngine
{
    interface ICalculatorStateFactory
    {
        ICalculatorState GetCalculatorState(IImmutableList<decimal> values, IOperation operation);
    }
}