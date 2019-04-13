using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using FluentAssertions;
using Calculator.CalculatorEngine;
using Calculator.CalculatorEngine.Implementations;

namespace Calculator.Tests.CalculatorEngine
{
    public class CalculatorTests
    {

        //Mocks
        private Mock<ICalculatorStateFactory> CalculatorStateFactoryMock { get; }

        public CalculatorTests()
        {
            //Create mock members
            CalculatorStateFactoryMock = new Mock<ICalculatorStateFactory>();

            //Setup state factory
            CalculatorStateFactoryMock.Setup(x => x.NewCalculatorState(It.IsAny<IImmutableList<decimal>>(), It.IsAny<IOperation>())).Returns(new Func<IImmutableList<decimal>, IOperation, ICalculatorState>(
                (values, operation) =>
                {
                    var calculatorStateMock = new Mock<ICalculatorState>();
                    calculatorStateMock.Setup(x => x.Values).Returns(values);
                    calculatorStateMock.Setup(x => x.ActiveOperation).Returns(operation);
                    return calculatorStateMock.Object;
                }));
        }


        //////////////////////////////////
        // Test specific inputs/outputs //
        //////////////////////////////////

        [Theory]  //Format: (ExpectedExceptionType, ExpectedResult, Inputs[])
        [InlineData(null, 1,    "1")] //Single value
        [InlineData(null, 1,    "1", "+")] //Value and operation
        [InlineData(null, 2,    "1", "+", "2")] //Last value
        [InlineData(null, 3,    "1", "+", "2", "=")] //Equals
        [InlineData(null, 3,    "1", "+", "2", "+")] //Previous operation result
        [InlineData(null, 6,    "1", "+", "2", "+", "3", "=")] //Chained operations      
        [InlineData(null, 20,   "2", "+", "3", "*", "4", "=")] //Chained operations ordered    
        [InlineData(null, 10,   "2", "*", "3", "+", "4", "=")] //Chained operations order reversed
        [InlineData(null, -1,   "1", "+", "-2", "=")] //Negatives
        [InlineData(null, 4,    "2", "^2")] //Unary operation
        [InlineData(null, 7,    "2", "^2", "+", "3", "=")] //Unary chained operation
        [InlineData(null, 9,    "1", "+", "3", "^2")] //Unary later operation
        [InlineData(null, 11,   "2", "+", "3", "^2", "=")] //Unary chained operation
        [InlineData(null, 12.1, "10", "CompoundInterest", ".1", "2", "=")] //Ternary operation
        [InlineData(null, 3.14, "PI")] //Nullary operation
        [InlineData(null, 4.14, "PI", "+", "1", "=")] //Nulary chained operation
        [InlineData(null, 5.14, "2", "+", "PI", "=")] //Nulary chained operation reversed
        //Invalid operations
        [InlineData(typeof(InvalidOperationException), 0, "+")] //Needs value first
        [InlineData(typeof(InvalidOperationException), 0, "=")] //Needs value first
        [InlineData(typeof(InvalidOperationException), 0, "^2")] //Missing operand
        [InlineData(typeof(InvalidOperationException), 0, "1", "1")] //Needs operation before second value
        [InlineData(typeof(InvalidOperationException), 0, "1", "=")] //Missing operation
        [InlineData(typeof(InvalidOperationException), 0, "1", "PI")] //Needs operation before second value
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "+")] //Incomplete operation
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "=")] //Incomplete operation
        //[InlineData(typeof(InvalidOperationException), 0, "1", "+", "^2")] //Missing operand to square
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "1", "1")] //Too many operands
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "1", "=", "1")] //Missing second operation
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "1", "=", "=")] //Missing second operation
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "1", "=", "PI")] //Missing second operation
        [InlineData(typeof(InvalidOperationException), 0, "1", "CompoundInterest", "1", "+")] //Incomplete operation
        [InlineData(typeof(InvalidOperationException), 0, "1", "CompoundInterest", "1", "=")] //Incomplete operation
        [InlineData(typeof(InvalidOperationException), 0, "1", "CompoundInterest", "1", "1", "1")] //Too many operands
        public void TestCalculatorInputResults(Type expectedExceptionType, decimal expectedResult, params string[] inputs)
        {
            //Arrange
            var calculator = CalculatorWithSubmissions();
            Exception thrownException = null;
            decimal lastResult = 0;

            //Act
            try
            {
                foreach (var input in inputs)
                    lastResult = resultOfSubmittedInput(input);
            }
            catch(Exception exception)
            {
                thrownException = exception;
            }

            //Assert
            if (expectedExceptionType != null)
            {
                thrownException.Should().BeAssignableTo(expectedExceptionType);
            }
            else
            {
                thrownException.Should().BeNull();
                lastResult.Should().Be(expectedResult);
            }

            //Local functions
            decimal resultOfSubmittedInput(string input)
                => input.Equals("+") ? calculator.SubmitOperationInputAndGetResult(additionOperation())
                    : input.Equals("*") ? calculator.SubmitOperationInputAndGetResult(multiplicationOperation())
                    : input.Equals("^2") ? calculator.SubmitOperationInputAndGetResult(squareOperation())
                    : input.Equals("PI") ? calculator.SubmitOperationInputAndGetResult(piOperation())
                    : input.Equals("CompoundInterest") ? calculator.SubmitOperationInputAndGetResult(compoundInterestOperation())
                    : input.Equals("=") ? calculator.SubmitEqualsRequestAndGetResult()
                    : calculator.SubmitValueInputAndGetResult(decimal.Parse(input));
            IOperation additionOperation()
                => Operation(numberOfOperands: 2, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands) 
                    => operands.ToList()[0] + operands.ToList()[1]));
            IOperation multiplicationOperation()
                => Operation(numberOfOperands: 2, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands) 
                    => operands.ToList()[0] * operands.ToList()[1]));
            IOperation squareOperation()
                => Operation(numberOfOperands: 1, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands) 
                    => operands.ToList()[0] * operands.ToList()[0]));
            IOperation piOperation()
                => Operation(numberOfOperands: 0, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands) 
                    => 3.14M));
            IOperation compoundInterestOperation()
                => Operation(numberOfOperands: 3, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands)
                    => operands.ToList()[0] * (decimal)Math.Pow((double)(1 + operands.ToList()[1]), (double)operands.ToList()[2])));
        }

        private Calculator.CalculatorEngine.Implementations.Calculator CalculatorWithSubmissions(params object[] inputs)
        {
            //Get initial calculator
            var calculator = new Calculator.CalculatorEngine.Implementations.Calculator(CalculatorStateFactoryMock.Object);
            
            //Execute state updates
            foreach (var input in inputs)
            {
                if (input is decimal || input is int || input is double)
                    calculator.SubmitValueInputAndGetResult(decimal.Parse(input.ToString()));   
                else if (input is IOperation operation)
                    calculator.SubmitOperationInputAndGetResult((IOperation)operation);
                else
                    calculator.SubmitEqualsRequestAndGetResult();
            }

            //Return
            return calculator;
        }

        private IOperation Operation(int numberOfOperands, decimal operationResult = 0, Func<IEnumerable<decimal>, decimal> resultFunction = null)
        {
            //Setup number of operands
            var operationMock = new Mock<IOperation>();
            operationMock.Setup(x => x.NumberOfOperands()).Returns(numberOfOperands);
            
            //Setup result for operands
            if (resultFunction != null)
                operationMock.Setup(x => x.ResultForOperands(It.IsAny<IEnumerable<decimal>>())).Returns(resultFunction);
            else
                operationMock.Setup(x => x.ResultForOperands(It.IsAny<IList<decimal>>())).Returns(operationResult);

            //Return operation
            var operation = operationMock.Object;
            return operation;
        }


        ////////////////////////////////
        // Test general functionality //
        ////////////////////////////////

        [Fact]
        public void SubmitValueInputAndGetResult_InitialState_ReturnsInput()
        {
            //Assert
            resultOnInitialState().Should().Be(input());

            //Local functions
            decimal resultOnInitialState() 
                => CalculatorWithSubmissions().SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        [Fact]
        public void SubmitValueInputAndGetResult_AfterValueInput_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultAfterValueInput());

            //Local functions
            decimal resultAfterValueInput() 
                => CalculatorWithSubmissions(1).SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        [Fact]
        public void SubmitValueInputAndGetResult_AfterNullaryOperation_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultAfterNullaryOperation());

            //Local functions
            decimal resultAfterNullaryOperation() 
                => CalculatorWithSubmissions(Operation(0)).SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        [Fact]
        public void SubmitValueInputAndGetResult_AfterIncompleteOperation_ReturnsInput()
        {
            //Assert
            resultAfterIncompleteOperation().Should().Be(input());

            //Local functions
            decimal resultAfterIncompleteOperation() 
                => CalculatorWithSubmissions(1, Operation(2)).SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        [Fact]
        public void SubmitValueInputAndGetResult_AfterCompleteOperation_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultAfterCompleteOperation());

            //Local functions
            decimal resultAfterCompleteOperation()
                => CalculatorWithSubmissions(1, Operation(2), 1).SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_NullaryOperationOnInitialState_ReturnsOperationResult()
        {
            //Assert
            resultOfSubmittingNullaryOperationOnInitialState().Should().Be(operationResult());

            //Local functions
            decimal resultOfSubmittingNullaryOperationOnInitialState() 
                => CalculatorWithSubmissions().SubmitOperationInputAndGetResult(operation());
            IOperation operation()
                => Operation(operandCount(), operationResult());
            int operandCount()
                => 0;
            decimal operationResult()
                => 123;
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_NonNullaryOperationOnInitialState_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultOfSubmittingOperationWithOperandsOnInitialState());

            //Local functions
            decimal resultOfSubmittingOperationWithOperandsOnInitialState()
                => CalculatorWithSubmissions().SubmitOperationInputAndGetResult(operation());
            IOperation operation()
                => Operation(operandCount());
            int operandCount()
                => 1;
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_UnaryOperationAfterValueInput_ReturnsOperationResult()
        {
            //Assert
            resultOfOperationWithSingleOperandAfterValueInput().Should().Be(operationResult());

            //Local functions
            decimal resultOfOperationWithSingleOperandAfterValueInput()
                => CalculatorWithSubmissions(1).SubmitOperationInputAndGetResult(operationWithSingleOperand());
            IOperation operationWithSingleOperand()
                => Operation(1, operationResult());
            decimal operationResult()
                => 123;
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_MultipleOperandOperationAfterValueInput_ReturnsOriginalValue()
        {
            //Assert
            resultOfMultiOperandOperationAfterValueInput().Should().Be(originalValue());

            //Local functions
            decimal resultOfMultiOperandOperationAfterValueInput()
                => CalculatorWithSubmissions(originalValue()).SubmitOperationInputAndGetResult(multiOperandOperation());
            decimal originalValue()
                => 456;
            IOperation multiOperandOperation()
                => Operation(2, operationResult());
            decimal operationResult()
                => 123;
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_MultiOperandOperationAfterIncompleteOperation_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultOfOperationAfterIncompleteOperation());

            //Local functions
            decimal resultOfOperationAfterIncompleteOperation()
                => CalculatorWithSubmissions(1, Operation(2)).SubmitOperationInputAndGetResult(multiOperandOperation());
            IOperation multiOperandOperation()
                => Operation(2);
        }

        [Fact]
        public void SubmitEqualsRequestAndGetResult_InitialState_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultOnInitialState());

            //Local functions
            decimal resultOnInitialState()
                => CalculatorWithSubmissions().SubmitEqualsRequestAndGetResult();
        }

        [Fact]
        public void SubmitEqualsRequestAndGetResult_AfterValueInput_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultAfterValueInput());

            //Local functions
            decimal resultAfterValueInput()
                => CalculatorWithSubmissions(0).SubmitEqualsRequestAndGetResult();
        }

        [Fact]
        public void SubmitEqualsRequestAndGetResult_AfterIncompleteOperation_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultofEqualsAfterIncompleteOperation());

            //Local functions
            decimal resultofEqualsAfterIncompleteOperation()
                => CalculatorWithSubmissions(1, Operation(2)).SubmitEqualsRequestAndGetResult();
        }

        [Fact]
        public void SubmitEqualsRequestAndGetResult_AfterCompleteOperation_ReturnsOperationResult()
        {
            //Assert
            resultofEqualsAfterIncompleteOperation().Should().Be(operationResult());

            //Local functions
            decimal resultofEqualsAfterIncompleteOperation()
                => CalculatorWithSubmissions(1, binaryOperation(), 2).SubmitEqualsRequestAndGetResult();
            IOperation binaryOperation()
                => Operation(2, operationResult());
            decimal operationResult()
                => 123;
        }
    }
}