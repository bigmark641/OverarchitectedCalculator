using System;
using System.Collections.Immutable;

namespace Calculator
{
    interface ICalculatorStateFactory
    {
        ICalculatorState CreateCalculatorState(IImmutableList<decimal> values, IOperation operation);
    }
}