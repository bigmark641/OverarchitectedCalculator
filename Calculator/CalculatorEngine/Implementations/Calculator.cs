using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calculator.CalculatorEngine;

namespace Calculator.CalculatorEngine.Implementations
{
    public class Calculator : ICalculator
    {
        //Injected dependencies
        private ICalculatorStateFactory CalculatorStateFactory { get; }

        //Mutable application state
        private ICalculatorState CurrentState { get; set; }

        public Calculator(ICalculatorStateFactory calculatorStateFactory)
        {
            //Inject readonly dependencies
            CalculatorStateFactory = calculatorStateFactory;

            //Initialize mutable state
            CurrentState = CalculatorStateFactory.GetCalculatorState(initialValues(), initialOperation());
            IImmutableList<decimal> initialValues() => ImmutableList<decimal>.Empty;
            IOperation initialOperation() => null;
        }

        public decimal SubmitValueInputAndGetResult(decimal valueInput)
        {
            //Update mutable state for new value (statement with side effects)
            CurrentState = isValueInputValidForCurrentState()
                ? CalculatorStateFactory.GetCalculatorState(valuesAfterNewValueInput(), operationAfterNewValueInput())
                : throw new InvalidOperationException();
            
            //Return latest value
            return GetLatestValue();

            //Is valid?            
            bool isValueInputValidForCurrentState() => !HasReceivedValues() || hasIncompleteOperation();
            bool hasIncompleteOperation() => HasActiveOperation() && !IsActiveOperationComplete();
                        
            //Values and operation after new input
            IImmutableList<decimal> valuesAfterNewValueInput() => CurrentState.Values.Add(valueInput);
            IOperation operationAfterNewValueInput() => CurrentState.ActiveOperation;
        }

        private bool HasReceivedValues() 
            => CurrentState.Values.Any();

        private bool HasActiveOperation() 
            => CurrentState.ActiveOperation != null;

        private bool IsActiveOperationComplete()
            => CurrentState.Values.Count() == CurrentState.ActiveOperation.GetNumberOfOperands();

        public decimal SubmitOperationInputAndGetResult(IOperation newOperation)
        {
            //Update mutable state for new operation (statement with side effects)
            CurrentState = isOperationInputValidForCurrentState()
                ?   stateAfterNewOperationInput()
                : throw new InvalidOperationException();

            //Return latest value
            return GetLatestValue();         
            
            //Is valid?
            bool isOperationInputValidForCurrentState()
                => EvaluateFunctionForNewOperation(newOperation,
                    ifNoOperands: () 
                        => !HasReceivedValues() || HasActiveOperation(),
                    ifSingleOperand: () 
                        => HasReceivedValues(),
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
            //Update mutable state for equals request (statement with side effects)
            CurrentState = isEqualsRequestValidForCurrentState()
                    ? stateAfterEqualsRequestEvaluation()
                    : throw new InvalidOperationException();
            
            //Return latest value
            return GetLatestValue();    

            //Is valid?
            bool isEqualsRequestValidForCurrentState()
                => HasActiveOperation() && IsActiveOperationComplete();       
            //State after evaluation
            ICalculatorState stateAfterEqualsRequestEvaluation()
                => CalculatorStateFactory.GetCalculatorState(valuesAfterEqualsRequestEvaluation(), null);
            IImmutableList<decimal> valuesAfterEqualsRequestEvaluation()
                => GetListWithSingleValue(CurrentState.ActiveOperation.GetResultForOperands(CurrentState.Values));
        }
            
        private IImmutableList<decimal> GetCurrentValuesWithLastReplaced(decimal newValue)
            => CurrentState.Values.SetItem(CurrentState.Values.Count() - 1, newValue);
        
        private IImmutableList<decimal> GetCurrentValuesPlusNewValue(decimal newValue)
            => CurrentState.Values.Add(newValue);
        
        private IImmutableList<decimal> GetListWithSingleValue(decimal value)
            => ImmutableList<decimal>.Empty.Add(value);

        private decimal GetLatestValue()
            => CurrentState.Values.Last();
    }
}