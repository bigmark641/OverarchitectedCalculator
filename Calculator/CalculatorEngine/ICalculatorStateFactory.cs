using System;
using System.Collections.Immutable;

namespace Calculator.CalculatorEngine
{
    public interface ICalculatorStateFactory
    {
        ICalculatorState NewCalculatorState(IImmutableList<decimal> values, IOperation operation);
    }
}