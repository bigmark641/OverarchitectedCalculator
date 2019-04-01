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
            CurrentState = isValueInputValidForCurrentState()
                ? CalculatorStateFactory.GetCalculatorState(valuesAfterNewValueInput(), operationAfterNerValueInput())
                : throw new InvalidOperationException();
            
            //Return latest value
            return GetLatestValue();

            //Is valid?            
            bool isValueInputValidForCurrentState() => !CurrentState.Values.Any() || (CurrentState.ActiveOperation != null && !IsCurrentOperationComplete());
            
            //Values and operation after new input
            IImmutableList<decimal> valuesAfterNewValueInput() => CurrentState.Values.Add(valueInput);
            IOperation operationAfterNerValueInput() => CurrentState.ActiveOperation;
        }

        private bool IsCurrentOperationComplete()
            => CurrentState.Values.Count() == CurrentState.ActiveOperation.GetNumberOfOperands();

        public decimal SubmitOperationInputAndGetResult(IOperation newOperation)
        {
            //Update mutable state for new operation
            CurrentState = isOperationInputValidForCurrentState()
                ?   stateAfterNewOperationInput()
                : throw new InvalidOperationException();

            //Return latest value
            return GetLatestValue();         
            
            //Is valid?
            bool isOperationInputValidForCurrentState()
                => EvaluateFunctionForNewOperation(newOperation,
                    ifNoOperands: () 
                        => !CurrentState.Values.Any() || CurrentState.ActiveOperation != null,
                    ifSingleOperand: () 
                        => CurrentState.Values.Any(),
                    ifMultipleOperandsWithoutExistingOperation: () 
                        => CurrentState.Values.Count() == 1,
                    ifMultipleOperandsWithExistingOperation: () 
                        => CurrentState.Values.Count() == CurrentState.ActiveOperation.GetNumberOfOperands());

            //State after operation
            ICalculatorState stateAfterNewOperationInput()
                => CalculatorStateFactory.GetCalculatorState(valuesAfterNewOperationInput(), operationAfterNewOperationInput());
            IImmutableList<decimal> valuesAfterNewOperationInput()
                => EvaluateFunctionForNewOperation(newOperation,
                    ifNoOperands: ()
                        => GetCurrentValuesPlusNewValue(newOperation.GetResultForOperands(ImmutableList<decimal>.Empty)),
                    ifSingleOperand: () 
                        => GetCurrentValuesWithLastReplaced(newOperation.GetResultForOperands(GetListWithSingleValue(CurrentState.Values.Last()))),
                    ifMultipleOperandsWithoutExistingOperation: () 
                        => CurrentState.Values,
                    ifMultipleOperandsWithExistingOperation: () 
                        => GetListWithSingleValue(CurrentState.ActiveOperation.GetResultForOperands(CurrentState.Values)));
            IOperation operationAfterNewOperationInput()
                => EvaluateFunctionForNewOperation(newOperation,
                    ifNoOperands: () 
                        => CurrentState.ActiveOperation,
                    ifSingleOperand: () 
                        => CurrentState.ActiveOperation,
                    ifMultipleOperandsWithoutExistingOperation: () 
                        => newOperation,
                    ifMultipleOperandsWithExistingOperation: () 
                        => newOperation);  
        }

        private T EvaluateFunctionForNewOperation<T>(IOperation newOperation, Func<T> ifNoOperands,Func<T> ifSingleOperand, Func<T> ifMultipleOperandsWithoutExistingOperation, Func<T> ifMultipleOperandsWithExistingOperation)
            =>  newOperation.GetNumberOfOperands() == 0
                    ? ifNoOperands()
                : newOperation.GetNumberOfOperands() == 1
                    ? ifSingleOperand()
                : CurrentState.ActiveOperation == null
                    ? ifMultipleOperandsWithoutExistingOperation()
                    : ifMultipleOperandsWithExistingOperation();

        public decimal SubmitEqualsRequestAndGetResult()
        {
            //Update mutable state for equals request
            CurrentState = isEqualsRequestValidForCurrentState()
                    ? stateAfterEqualsRequestEvaluation()
                    : throw new InvalidOperationException();
            
            //Return latest value
            return GetLatestValue();    

            //Is valid?
            bool isEqualsRequestValidForCurrentState()
                => CurrentState.ActiveOperation != null && IsCurrentOperationComplete();       
            //State after evaluation
            ICalculatorState stateAfterEqualsRequestEvaluation()
                => CalculatorStateFactory.GetCalculatorState(valuesAfterEqualsRequestEvaluation(), null);
            IImmutableList<decimal> valuesAfterEqualsRequestEvaluation()
                => GetListWithSingleValue(CurrentState.ActiveOperation.GetResultForOperands(CurrentState.Values));
        }
            
        IImmutableList<decimal> GetCurrentValuesWithLastReplaced(decimal newValue)
            => CurrentState.Values.SetItem(CurrentState.Values.Count() - 1, newValue);
        
        IImmutableList<decimal> GetCurrentValuesPlusNewValue(decimal newValue)
            => CurrentState.Values.Add(newValue);
        
        IImmutableList<decimal> GetListWithSingleValue(decimal value)
            => ImmutableList<decimal>.Empty.Add(value);

        private decimal GetLatestValue()
            => CurrentState.Values.Last();
    }
}