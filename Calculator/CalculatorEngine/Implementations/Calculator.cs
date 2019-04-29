using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calculator.CalculatorEngine;
using Calculator.Utilities;

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
            IImmutableList<decimal> initialValues() 
                => ImmutableList<decimal>.Empty;
            IOperation initialOperation() 
                => null;
        }

        public Validated<decimal> SubmitValueInputAndGetResult(decimal valueInput)
        {
            //Validate, submit input, and get result
            return validatedValueInput(valueInput)
                .Map(submitAndGetResult);

            //Local functions
            Validated<decimal> validatedValueInput(decimal input)
                => IsAdditionalValueValidOnCurrentState()
                    ? new Validated<decimal>(input)
                    : new StringError("Value is not currently allowed.");
            decimal submitAndGetResult(decimal input) 
                => LatestValueOfState(CurrentState = stateAfter(input));
            ICalculatorState stateAfter(decimal input) 
                => CalculatorStateFactory.NewCalculatorState(CurrentValuesIncluding(input), activeOperation());
            IImmutableList<decimal> CurrentValuesIncluding(decimal input) 
                => CurrentState.Values.Add(input);
            IOperation activeOperation()
                => CurrentState.ActiveOperation;
        }

        private bool IsAdditionalValueValidOnCurrentState()
            => !HasReceivedValues() || HasIncompleteOperation();

        private bool HasReceivedValues() 
            => CurrentState.Values.Any();

        bool HasIncompleteOperation() 
            => HasActiveOperation() && !IsActiveOperationComplete();

        private bool HasActiveOperation() 
            => CurrentState.ActiveOperation != null;

        private bool IsActiveOperationComplete()
            => CurrentState.Values.Count() == CurrentState.ActiveOperation.NumberOfOperands();

        public Validated<decimal> SubmitOperationInputAndGetResult(IOperation operationInput)
        {
            //Validate, submit input, and get result
            return validatedOperationInput(operationInput)
                .Bind(submitAndGetResult);
            
            //Local functions
            Validated<IOperation> validatedOperationInput(IOperation input)
                => isValidOnCurrentState(input)
                    ? new Validated<IOperation>(input)
                    : new StringError("Operation is not currently allowed.");
            bool isValidOnCurrentState(IOperation input)
                => FunctionForNewOperation(input,
                    ifNoOperands: ()
                        => IsAdditionalValueValidOnCurrentState(),
                    ifSingleOperand: ()
                        => HasReceivedValues(),
                    ifMultipleOperandsWithoutActiveOperation: () 
                        => HasReceivedValues(),
                    ifMultipleOperandsWithActiveOperation: () 
                        => IsActiveOperationComplete());
            Validated<decimal> submitAndGetResult(IOperation input)
                => validatedEvaluationResult(input)
                    .Map(x => saveAndGetResult(input, x));
            Validated<decimal> validatedEvaluationResult(IOperation input)
                => FunctionForNewOperation(input,
                    ifNoOperands: ()
                        => input.ResultForOperands(ImmutableList<decimal>.Empty),
                    ifSingleOperand: ()
                        => input.ResultForOperands(ListWithSingleValue(CurrentState.Values.Last())),
                    ifMultipleOperandsWithoutActiveOperation: ()
                        => new Validated<decimal>(null),
                    ifMultipleOperandsWithActiveOperation: ()
                        => CurrentState.ActiveOperation.ResultForOperands(CurrentState.Values));
            decimal saveAndGetResult(IOperation evaluatedOperation, decimal resultingValue)
                => LatestValueOfState(CurrentState = resultingState(evaluatedOperation, resultingValue));
            ICalculatorState resultingState(IOperation evaluatedOperation, decimal resultingValue)
                => CalculatorStateFactory.NewCalculatorState(resultingValues(evaluatedOperation, resultingValue), newOperation(evaluatedOperation));
            IImmutableList<decimal> resultingValues(IOperation evaluatedOperation, decimal resultingValue)
                => FunctionForNewOperation(evaluatedOperation,
                    ifNoOperands: ()
                        => CurrentValuesPlusNewValue(resultingValue),
                    ifSingleOperand: () 
                        => CurrentValuesWithLastReplaced(resultingValue),
                    ifMultipleOperandsWithoutActiveOperation: () 
                        => CurrentState.Values,
                    ifMultipleOperandsWithActiveOperation: () 
                        => ListWithSingleValue(resultingValue));
            IOperation newOperation(IOperation evaluatedOperation)
                => FunctionForNewOperation(evaluatedOperation,
                    ifNoOperands: () 
                        => CurrentState.ActiveOperation,
                    ifSingleOperand: () 
                        => CurrentState.ActiveOperation,
                    ifMultipleOperandsWithoutActiveOperation: () 
                        => evaluatedOperation,
                    ifMultipleOperandsWithActiveOperation: () 
                        => evaluatedOperation);  
        }

        private T FunctionForNewOperation<T>(IOperation newOperation, Func<T> ifNoOperands,Func<T> ifSingleOperand, Func<T> ifMultipleOperandsWithoutActiveOperation, Func<T> ifMultipleOperandsWithActiveOperation)
            =>  newOperation.NumberOfOperands() == 0
                    ? ifNoOperands()
                : newOperation.NumberOfOperands() == 1
                    ? ifSingleOperand()
                : CurrentState.ActiveOperation == null
                    ? ifMultipleOperandsWithoutActiveOperation()
                    : ifMultipleOperandsWithActiveOperation();

        public Validated<decimal> SubmitEqualsRequestAndGetResult()
        {
            //Validate, submit, and get result
            return validatedEqualsRequest()
                .Bind(submitEqualsRequestAndGetResult);

            //Local functions
            Validated validatedEqualsRequest()
                => isEqualsRequestValidForCurrentState()
                    ? new Validated()
                    : new StringError("Equals request is not currently allowed.");
            bool isEqualsRequestValidForCurrentState()
                => HasActiveOperation() && IsActiveOperationComplete(); 
            Validated<decimal> submitEqualsRequestAndGetResult()
                => validatedEqualsRequestResult()
                    .Map(saveAndGetResult);
            decimal saveAndGetResult(decimal result)
                => LatestValueOfState(CurrentState = stateWithEqualsRequestResult(result));
            ICalculatorState stateWithEqualsRequestResult(decimal equalsRequestResult)
                => CalculatorStateFactory.NewCalculatorState(ListWithSingleValue(equalsRequestResult), null);
            Validated<decimal> validatedEqualsRequestResult()
                => CurrentState.ActiveOperation.ResultForOperands(CurrentState.Values);
        }
            
        private IImmutableList<decimal> CurrentValuesWithLastReplaced(decimal newValue)
            => CurrentState.Values.SetItem(CurrentState.Values.Count() - 1, newValue);
        
        private IImmutableList<decimal> CurrentValuesPlusNewValue(decimal newValue)
            => CurrentState.Values.Add(newValue);
        
        private IImmutableList<decimal> ListWithSingleValue(decimal value)
            => ImmutableList<decimal>.Empty.Add(value);

        private decimal LatestValueOfState(ICalculatorState calculatorState)
            => calculatorState.Values.Last();
    }
}