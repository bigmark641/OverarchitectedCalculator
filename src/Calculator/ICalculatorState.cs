using System;
using System.Collections.Immutable;

namespace Calculator
{
    interface ICalculatorState
    {
        IImmutableList<decimal> Values {get;}
        IOperation Operation {get;}
    }
}