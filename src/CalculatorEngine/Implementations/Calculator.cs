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
            //Update mutable state for new value
            CurrentState = IsValueInputValidForCurrentState()
                ? CalculatorStateFactory.GetCalculatorState(currentValuesWithNewInput(), currentOperation())
                : throw new ArgumentException();
            
            //Return latest value
            return GetLatestValue();
            IImmutableList<decimal> currentValuesWithNewInput() => CurrentState.Values.Add(valueInput);
            IOperation currentOperation() => CurrentState.ActiveOperation;
        }

        private bool IsValueInputValidForCurrentState()
            => !CurrentState.Values.Any() || CurrentState.ActiveOperation != null;

        public decimal SubmitOperationInputAndGetResult(IOperation newOperation)
        {
            //Update mutable state for new operation
            CurrentState = IsOperationInputValidForCurrentState(newOperation)
                ?   GetStateAfterNewOperationInput(newOperation)
                : throw new ArgumentException();

            //Return latest value
            return GetLatestValue();           
        }

        private bool IsOperationInputValidForCurrentState(IOperation newOperation)
            =>  newOperation.GetNumberOfOperands() == 0
                    ? !CurrentState.Values.Any() || CurrentState.ActiveOperation != null
                : newOperation.GetNumberOfOperands() == 1
                    ? CurrentState.Values.Any()
                : CurrentState.ActiveOperation == null
                    ? CurrentState.Values.Count() == 1
                    : CurrentState.Values.Count() == CurrentState.ActiveOperation.GetNumberOfOperands();

        private ICalculatorState GetStateAfterNewOperationInput(IOperation newOperation)
            => CalculatorStateFactory.GetCalculatorState(GetValuesAfterNewOperationInput(newOperation), GetOperationAfterNewOperationInput(newOperation));

        private IImmutableList<decimal> GetValuesAfterNewOperationInput(IOperation newOperation)
            =>  newOperation.GetNumberOfOperands() == 0
                    ? GetCurrentValuesPlusNewValue(newOperation.GetResultForOperands(ImmutableList<decimal>.Empty))
                : newOperation.GetNumberOfOperands() == 1
                    ? GetCurrentValuesWithLastReplaced(newOperation.GetResultForOperands(GetListWithSingleValue(CurrentState.Values.Last())))
                : CurrentState.ActiveOperation == null
                    ? CurrentState.Values
                    : GetListWithSingleValue(CurrentState.ActiveOperation.GetResultForOperands(CurrentState.Values));

        private IOperation GetOperationAfterNewOperationInput(IOperation newOperation)
            =>  newOperation.GetNumberOfOperands() == 0
                    ? CurrentState.ActiveOperation
                : newOperation.GetNumberOfOperands() == 1
                    ? CurrentState.ActiveOperation
                : CurrentState.ActiveOperation == null
                    ? newOperation
                    : newOperation;
            
        IImmutableList<decimal> GetCurrentValuesWithLastReplaced(decimal newValue)
            => CurrentState.Values.SetItem(CurrentState.Values.Count()-1, newValue);
        
        IImmutableList<decimal> GetCurrentValuesPlusNewValue(decimal newValue)
            => CurrentState.Values.Add(newValue);
        
        IImmutableList<decimal> GetListWithSingleValue(decimal value)
            => ImmutableList<decimal>.Empty.Add(value);

        private decimal GetLatestValue()
            => CurrentState.Values.Last();

        public decimal SubmitEqualsRequestAndGetResult()
        {
            //Update mutable state for equals request
            CurrentState = stateAfterEqualsRequestEvaluation();

            ICalculatorState stateAfterEqualsRequestEvaluation()
                => isEqualsRequestValidForCurrentState()
                    ? CalculatorStateFactory.GetCalculatorState(GetListWithSingleValue(CurrentState.ActiveOperation.GetResultForOperands(CurrentState.Values)), null)
                    : throw new ArgumentException();
            bool isEqualsRequestValidForCurrentState()
                => CurrentState.ActiveOperation != null && CurrentState.Values.Count() == CurrentState.ActiveOperation.GetNumberOfOperands();
            
            //Return latest value
            return GetLatestValue();           
        }
    }
}