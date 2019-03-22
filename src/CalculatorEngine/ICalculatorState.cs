using System;
using System.Collections.Immutable;

namespace CalculatorEngine
{
    interface ICalculatorState
    {
        IImmutableList<decimal> Values {get;}
        IOperation Operation {get;}
    }
}