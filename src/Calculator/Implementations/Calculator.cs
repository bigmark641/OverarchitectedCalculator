using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calculator;

namespace Calculator.Implementations
{
    class Calculator : ICalculator
    {
        private ICalculatorStateFactory CalculatorStateFactory { get; }

        //Mutable application state
        private ICalculatorState CurrentState { get; set; }

        public Calculator(ICalculatorStateFactory calculatorStateFactory)
        {
            CalculatorStateFactory = calculatorStateFactory;
            //Initialize state
            CurrentState = CalculatorStateFactory.GetCalculatorState(initialValues(), initialOperation());
            IImmutableList<decimal> initialValues() => ImmutableList<decimal>.Empty;
            IOperation initialOperation() => null;
        }

        public decimal SubmitValueInputAndGetResult(decimal valueInput)
        {
            //Update state
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
            //Update state
            CurrentState = GetStateAfterOperationEvaluation(currentValues(), newlyInputtedOperation());
            //Return latest value
            return GetLatestValue();
            IImmutableList<decimal> currentValues() => CurrentState.Values;
            IOperation newlyInputtedOperation() => operationInput;            
        }

        private ICalculatorState GetStateAfterOperationEvaluation(IImmutableList<decimal> values, IOperation operation)
        {
            return CalculatorStateFactory.GetCalculatorState(valuesAfterOperationEvaluation(), operationAfterOperationEvaluation());
            IImmutableList<decimal> valuesAfterOperationEvaluation() => canExecuteOperation()
                ? ImmutableList<decimal>.Empty.Add(resultForOperation())
                : values;
            bool canExecuteOperation() => hasOperation() && operation.GetNumberOfOperands() == values.Count();
            bool hasOperation() => operation != null;
            decimal resultForOperation() => operation.GetResultForOperands(valueList());
            IList<decimal> valueList() => values.ToList();
            IOperation operationAfterOperationEvaluation() => canExecuteOperation()
                ? null
                : operation;
        }
    }
}