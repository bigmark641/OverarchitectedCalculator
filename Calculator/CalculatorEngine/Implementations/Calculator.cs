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
            CurrentState = CalculatorStateFactory.NewCalculatorState(initialValues(), initialOperation());

            //Local functions 
            IImmutableList<decimal> initialValues() => ImmutableList<decimal>.Empty;
            IOperation initialOperation() => null;
        }

        public decimal SubmitValueInputAndGetResult(decimal valueInput)
        {
            //Update mutable state for new value (statement with side effects)
            CurrentState = isValueInputValidForCurrentState()
                ? CalculatorStateFactory.NewCalculatorState(valuesAfterNewValueInput(), operationAfterNewValueInput())
                : throw new InvalidOperationException();
            
            //Return latest value
            return LatestValue();

            //Local functions
            bool isValueInputValidForCurrentState() => !HasReceivedValues() || hasIncompleteOperation();
            bool hasIncompleteOperation() => HasActiveOperation() && !IsActiveOperationComplete();
            IImmutableList<decimal> valuesAfterNewValueInput() => CurrentState.Values.Add(valueInput);
            IOperation operationAfterNewValueInput() => CurrentState.ActiveOperation;
        }

        private bool HasReceivedValues() 
            => CurrentState.Values.Any();

        private bool HasActiveOperation() 
            => CurrentState.ActiveOperation != null;

        private bool IsActiveOperationComplete()
            => CurrentState.Values.Count() == CurrentState.ActiveOperation.NumberOfOperands();

        public decimal SubmitOperationInputAndGetResult(IOperation newOperation)
        {
            //Update mutable state for new operation (statement with side effects)
            CurrentState = isOperationInputValidForCurrentState()
                ?   stateAfterNewOperationInput()
                : throw new InvalidOperationException();

            //Return latest value
            return LatestValue();         
            
            //Local functions
            bool isOperationInputValidForCurrentState()
                => FunctionForNewOperation(newOperation,
                    ifNoOperands: () 
                        => !HasReceivedValues() || HasActiveOperation(),
                    ifSingleOperand: () 
                        => HasReceivedValues(),
                    ifMultipleOperandsWithoutExistingOperation: () 
                        => CurrentState.Values.Count() == 1,
                    ifMultipleOperandsWithExistingOperation: () 
                        => CurrentState.Values.Count() == CurrentState.ActiveOperation.NumberOfOperands());
            ICalculatorState stateAfterNewOperationInput()
                => CalculatorStateFactory.NewCalculatorState(valuesAfterNewOperationInput(), operationAfterNewOperationInput());
            IImmutableList<decimal> valuesAfterNewOperationInput()
                => FunctionForNewOperation(newOperation,
                    ifNoOperands: ()
                        => CurrentValuesPlusNewValue(newOperation.ResultForOperands(ImmutableList<decimal>.Empty)),
                    ifSingleOperand: () 
                        => CurrentValuesWithLastReplaced(newOperation.ResultForOperands(ListWithSingleValue(CurrentState.Values.Last()))),
                    ifMultipleOperandsWithoutExistingOperation: () 
                        => CurrentState.Values,
                    ifMultipleOperandsWithExistingOperation: () 
                        => ListWithSingleValue(CurrentState.ActiveOperation.ResultForOperands(CurrentState.Values)));
            IOperation operationAfterNewOperationInput()
                => FunctionForNewOperation(newOperation,
                    ifNoOperands: () 
                        => CurrentState.ActiveOperation,
                    ifSingleOperand: () 
                        => CurrentState.ActiveOperation,
                    ifMultipleOperandsWithoutExistingOperation: () 
                        => newOperation,
                    ifMultipleOperandsWithExistingOperation: () 
                        => newOperation);  
        }

        private T FunctionForNewOperation<T>(IOperation newOperation, Func<T> ifNoOperands,Func<T> ifSingleOperand, Func<T> ifMultipleOperandsWithoutExistingOperation, Func<T> ifMultipleOperandsWithExistingOperation)
            =>  newOperation.NumberOfOperands() == 0
                    ? ifNoOperands()
                : newOperation.NumberOfOperands() == 1
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
            return LatestValue();    

            //Local functions
            bool isEqualsRequestValidForCurrentState()
                => HasActiveOperation() && IsActiveOperationComplete();       
            ICalculatorState stateAfterEqualsRequestEvaluation()
                => CalculatorStateFactory.NewCalculatorState(valuesAfterEqualsRequestEvaluation(), null);
            IImmutableList<decimal> valuesAfterEqualsRequestEvaluation()
                => ListWithSingleValue(CurrentState.ActiveOperation.ResultForOperands(CurrentState.Values));
        }
            
        private IImmutableList<decimal> CurrentValuesWithLastReplaced(decimal newValue)
            => CurrentState.Values.SetItem(CurrentState.Values.Count() - 1, newValue);
        
        private IImmutableList<decimal> CurrentValuesPlusNewValue(decimal newValue)
            => CurrentState.Values.Add(newValue);
        
        private IImmutableList<decimal> ListWithSingleValue(decimal value)
            => ImmutableList<decimal>.Empty.Add(value);

        private decimal LatestValue()
            => CurrentState.Values.Last();
    }
}