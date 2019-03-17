using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calculator;

namespace Calculator.Implementations
{
    class Calculator : ICalculator
    {
        private ICalculatorStateFactory CalculatorStateFactory {get;}

        //Mutable application state
        private ICalculatorState CurrentState {get; set;}

        public Calculator(ICalculatorStateFactory calculatorStateFactory)
        {
            CalculatorStateFactory = calculatorStateFactory;
            //Initialize state
            CurrentState = CalculatorStateFactory.CreateCalculatorState(ImmutableList<decimal>.Empty, null);
        }

        public decimal SubmitValueInputAndGetResult(decimal valueInput)
        {
            var currentValuesWithNewInput = CurrentState.Values.Add(valueInput);
            //Update state
            CurrentState = GetStateAfterOperationEvaluation(currentValuesWithNewInput, CurrentState.Operation);
            //Return last value
            return CurrentState.Values.Last();
        }

        public decimal SubmitOperationInputAndGetResult(IOperation operationInput)
        {
            //Update state
            CurrentState = GetStateAfterOperationEvaluation(CurrentState.Values, operationInput);
            //Return last value
            return CurrentState.Values.Last();
        }

        private ICalculatorState GetStateAfterOperationEvaluation(IImmutableList<decimal> values, IOperation operation)
        {
            var hasOperation = operation != null;
            var canExecuteOperation = hasOperation && operation.GetNumberOfOperands() == values.Count();
            var valuesAfterOperationEvaluation = canExecuteOperation
                ? ImmutableList<decimal>.Empty.Add(operation.GetResultForOperands(values.ToArray()))
                : values;
            var operationAfterOperationEvaluation = canExecuteOperation
                ? null
                : operation;
            return CalculatorStateFactory.CreateCalculatorState(valuesAfterOperationEvaluation, operation);
        }
    }
}