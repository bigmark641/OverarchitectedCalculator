using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CalculatorEngine;

namespace CalculatorEngine.Implementations
{
    class Calculator : ICalculator
    {
        private ICalculatorStateFactory CalculatorStateFactory { get; }

        //Mutable application state
        private ICalculatorState CurrentState { get; set; }

        public Calculator(ICalculatorStateFactory calculatorStateFactory)
        {
            //Set injected dependency
            CalculatorStateFactory = calculatorStateFactory;

            //Initialize mutable state
            CurrentState = CalculatorStateFactory.GetCalculatorState(initialValues(), initialOperation());
            IImmutableList<decimal> initialValues() => ImmutableList<decimal>.Empty;
            IOperation initialOperation() => null;
        }

        public decimal SubmitValueInputAndGetResult(decimal valueInput)
        {
            //Update mutable state
            CurrentState = GetStateAfterOperationEvaluation(currentValuesWithNewInput(), currentOperation());
            
            //Return latest value
            return GetLatestValue();
            IImmutableList<decimal> currentValuesWithNewInput() => CurrentState.Values.Add(valueInput);
            IOperation currentOperation() => CurrentState.Operation;
        }

        private decimal GetLatestValue()
            => CurrentState.Values.Last();

        public decimal SubmitOperationInputAndGetResult(IOperation operationInput)
        {
            //Update mutable state
            CurrentState = GetStateAfterOperationEvaluation(currentValues(), newlyInputtedOperation());
            
            //Return latest value
            return GetLatestValue();
            IImmutableList<decimal> currentValues() => CurrentState.Values;
            IOperation newlyInputtedOperation() => operationInput;            
        }

        private ICalculatorState GetStateAfterOperationEvaluation(IImmutableList<decimal> values, IOperation operation)
        {
            return CalculatorStateFactory.GetCalculatorState(valuesAfterOperationEvaluation(), operationAfterOperationEvaluation());
            
            //Get values after evaluation
            IImmutableList<decimal> valuesAfterOperationEvaluation() => canExecuteOperation()
                ? ImmutableList<decimal>.Empty.Add(resultForOperation())
                : values.Any()
                    ? values
                    : throw new ArgumentException();
            bool canExecuteOperation() => hasOperation() && operation.GetNumberOfOperands() == values.Count();
            bool hasOperation() => operation != null;

            //Get result of operation
            decimal resultForOperation() => operation.GetResultForOperands(valueList());
            IList<decimal> valueList() => values.ToList();

            //Get operation after evaluation
            IOperation operationAfterOperationEvaluation() => canExecuteOperation()
                ? null
                : operation;
        }
    }
}