using System;
using System.Collections.Immutable;

namespace Calculator
{
    interface ICalculatorStateFactory
    {
        ICalculatorState GetCalculatorState(IImmutableList<decimal> values, IOperation operation);
    }
}