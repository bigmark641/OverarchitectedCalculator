using System;
using System.Collections.Immutable;

namespace Calculator.CalculatorEngine
{
    public interface ICalculatorState
    {
        IImmutableList<decimal> Values {get;}
        IOperation ActiveOperation {get;}
    }
}