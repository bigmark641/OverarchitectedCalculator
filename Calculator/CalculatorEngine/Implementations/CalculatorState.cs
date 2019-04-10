using System;
using System.Collections.Immutable;
using Calculator.CalculatorEngine;

namespace Calculator.CalculatorEngine.Implementations
{
    public class CalculatorState : ICalculatorState
    {
        public IImmutableList<decimal> Values {get;}
        public IOperation ActiveOperation {get;}

        public CalculatorState(IImmutableList<decimal> values, IOperation operation)
        {

            //Initialize readonly state
            Values = values;
            ActiveOperation = operation;
        }
    }
}