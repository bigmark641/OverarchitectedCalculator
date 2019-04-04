using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using FluentAssertions;
using CalculatorEngine.Implementations;

namespace CalculatorEngine.Tests
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
            CalculatorStateFactoryMock.Setup(x => x.GetCalculatorState(It.IsAny<IImmutableList<decimal>>(), It.IsAny<IOperation>())).Returns(new Func<IImmutableList<decimal>, IOperation, ICalculatorState>(
                (values, operation) =>
                {
                    var calculatorStateMock = new Mock<ICalculatorState>();
                    calculatorStateMock.Setup(x => x.Values).Returns(values);
                    calculatorStateMock.Setup(x => x.ActiveOperation).Returns(operation);
                    return calculatorStateMock.Object;
                }));
        }

        [Fact]
        public void SubmitValueInputAndGetResult_InitialState_ReturnsInput()
        {
            //Assert
            resultOnInitialState().Should().Be(input());

            //Result on initial state
            decimal resultOnInitialState() 
                => GetCalculatorWithSubmissions().SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        private Implementations.Calculator GetCalculatorWithSubmissions(params object[] inputs)
        {
            //Get initial calculator
            var calculator = new Implementations.Calculator(CalculatorStateFactoryMock.Object);
            
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

        [Fact]
        public void SubmitValueInputAndGetResult_AfterValueInput_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultAfterValueInput());

            //Result on initial state
            decimal resultAfterValueInput() 
                => GetCalculatorWithSubmissions(1).SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        [Fact]
        public void SubmitValueInputAndGetResult_AfterNullaryOperation_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultAfterNullaryOperation());

            //Result on initial state
            decimal resultAfterNullaryOperation() 
                => GetCalculatorWithSubmissions(GetOperation(0)).SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        [Fact]
        public void SubmitValueInputAndGetResult_AfterIncompleteOperation_ReturnsInput()
        {
            //Assert
            resultAfterIncompleteOperation().Should().Be(input());

            //Result on initial state
            decimal resultAfterIncompleteOperation() 
                => GetCalculatorWithSubmissions(1, GetOperation(2)).SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        [Fact]
        public void SubmitValueInputAndGetResult_AfterCompleteOperation_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultAfterCompleteOperation());

            //Result on initial state
            decimal resultAfterCompleteOperation()
                => GetCalculatorWithSubmissions(1, GetOperation(2), 1).SubmitValueInputAndGetResult(input());
            decimal input()
                => 123;            
        }

        private IOperation GetOperation(int numberOfOperands, decimal operationResult = 0, Func<IEnumerable<decimal>, decimal> resultFunction = null)
        {
            var operationMock = new Mock<IOperation>();
            operationMock.Setup(x => x.GetNumberOfOperands()).Returns(numberOfOperands);
            
            if (resultFunction != null)
                operationMock.Setup(x => x.GetResultForOperands(It.IsAny<IEnumerable<decimal>>())).Returns(resultFunction);
            else
                operationMock.Setup(x => x.GetResultForOperands(It.IsAny<IList<decimal>>())).Returns(operationResult);

            var operation = operationMock.Object;
            return operation;
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_NullaryOperationOnInitialState_ReturnsOperationResult()
        {
            //Assert
            resultOfSubmittingNullaryOperationOnInitialState().Should().Be(operationResult());

            //Result of zero operand operation on initial state
            decimal resultOfSubmittingNullaryOperationOnInitialState() 
                => GetCalculatorWithSubmissions().SubmitOperationInputAndGetResult(operation());
            IOperation operation()
                => GetOperation(operandCount(), operationResult());
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

            //Result operation with operands on initial state
            decimal resultOfSubmittingOperationWithOperandsOnInitialState()
                => GetCalculatorWithSubmissions().SubmitOperationInputAndGetResult(operation());
            IOperation operation()
                => GetOperation(operandCount());
            int operandCount()
                => 1;
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_UnaryOperationAfterValueInput_ReturnsOperationResult()
        {
            //Assert
            resultOfOperationWithSingleOperandAfterValueInput().Should().Be(operationResult());

            //result of operation with single operand after value input
            decimal resultOfOperationWithSingleOperandAfterValueInput()
                => GetCalculatorWithSubmissions(1).SubmitOperationInputAndGetResult(operationWithSingleOperand());
            IOperation operationWithSingleOperand()
                => GetOperation(1, operationResult());
            decimal operationResult()
                => 123;
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_MultipleOperandOperationAfterValueInput_ReturnsOriginalValue()
        {
            //Assert
            resultOfMultiOperandOperationAfterValueInput().Should().Be(originalValue());

            //result of operation with single operand after value input
            decimal resultOfMultiOperandOperationAfterValueInput()
                => GetCalculatorWithSubmissions(originalValue()).SubmitOperationInputAndGetResult(multiOperandOperation());
            decimal originalValue()
                => 456;
            IOperation multiOperandOperation()
                => GetOperation(2, operationResult());
            decimal operationResult()
                => 123;
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_MultiOperandOperationAfterIncompleteOperation_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultOfOperationAfterIncompleteOperation());

            //result of operation with single operand after value input
            decimal resultOfOperationAfterIncompleteOperation()
                => GetCalculatorWithSubmissions(1, GetOperation(2)).SubmitOperationInputAndGetResult(multiOperandOperation());
            IOperation multiOperandOperation()
                => GetOperation(2);
        }

        [Fact]
        public void SubmitEqualsRequestAndGetResult_InitialState_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultOnInitialState());

            //Result on initial state
            decimal resultOnInitialState()
                => GetCalculatorWithSubmissions().SubmitEqualsRequestAndGetResult();
        }

        [Fact]
        public void SubmitEqualsRequestAndGetResult_AfterValueInput_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultAfterValueInput());

            //Result
            decimal resultAfterValueInput()
                => GetCalculatorWithSubmissions(0).SubmitEqualsRequestAndGetResult();
        }

        [Fact]
        public void SubmitEqualsRequestAndGetResult_AfterIncompleteOperation_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<InvalidOperationException>(() => resultofEqualsAfterIncompleteOperation());

            //Result
            decimal resultofEqualsAfterIncompleteOperation()
                => GetCalculatorWithSubmissions(1, GetOperation(2)).SubmitEqualsRequestAndGetResult();
        }

        [Fact]
        public void SubmitEqualsRequestAndGetResult_AfterCompleteOperation_ReturnsOperationResult()
        {
            //Assert
            resultofEqualsAfterIncompleteOperation().Should().Be(operationResult());

            //Result
            decimal resultofEqualsAfterIncompleteOperation()
                => GetCalculatorWithSubmissions(1, binaryOperation(), 2).SubmitEqualsRequestAndGetResult();
            IOperation binaryOperation()
                => GetOperation(2, operationResult());
            decimal operationResult()
                => 123;
        }

        [Theory]
        [InlineData(null, 1, "1")] //Single value
        [InlineData(null, 1, "1", "+")] //Value and operation
        [InlineData(null, 2, "1", "+", "2")] //Last value
        [InlineData(null, 3, "1", "+", "2", "=")] //Equals
        [InlineData(null, 3, "1", "+", "2", "+")] //Previous operation result
        [InlineData(null, 6, "1", "+", "2", "+", "3", "=")] //Chained operations      
        [InlineData(null, 20, "2", "+", "3", "*", "4", "=")] //Chained operations ordered    
        [InlineData(null, 10, "2", "*", "3", "+", "4", "=")] //Chained operations order reversed
        [InlineData(null, -1, "1", "+", "-2", "=")] //Negatives
        [InlineData(null, 4, "2", "^2")] //Unary operation
        [InlineData(null, 7, "2", "^2", "+", "3", "=")] //Unary chained operation
        [InlineData(null, 9, "1", "+", "3", "^2")] //Unary later operation
        [InlineData(null, 11, "2", "+", "3", "^2", "=")] //Unary chained operation
        [InlineData(null, 12.1, "10", "CompoundInterest", ".1", "2", "=")] //Ternary operation
        [InlineData(null, 3.14, "PI")] //Nullary operation
        [InlineData(null, 4.14, "PI", "+", "1", "=")] //Nulary chained operation
        [InlineData(null, 5.14, "2", "+", "PI", "=")] //Nulary chained operation reversed
        //Invalid operations
        [InlineData(typeof(InvalidOperationException), 0, "+")]
        [InlineData(typeof(InvalidOperationException), 0, "1", "1")]
        [InlineData(typeof(InvalidOperationException), 0, "1", "PI")]
        [InlineData(typeof(InvalidOperationException), 0, "=")]
        [InlineData(typeof(InvalidOperationException), 0, "1", "=")]
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "=")]
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "+")]
        [InlineData(typeof(InvalidOperationException), 0, "^2")]
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "1", "=", "=")]
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "1", "=", "1")]
        [InlineData(typeof(InvalidOperationException), 0, "1", "+", "1", "=", "PI")]
        public void TestCalculatorValidResults(Type expectedExceptionType, decimal expectedResult, params string[] inputs)
        {
            //Arrange
            var calculator = GetCalculatorWithSubmissions();
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

            //Result of submitted input
            decimal resultOfSubmittedInput(string input)
                => input.Equals("+") ? calculator.SubmitOperationInputAndGetResult(additionOperation())
                    : input.Equals("*") ? calculator.SubmitOperationInputAndGetResult(multiplicationOperation())
                    : input.Equals("^2") ? calculator.SubmitOperationInputAndGetResult(squareOperation())
                    : input.Equals("PI") ? calculator.SubmitOperationInputAndGetResult(piOperation())
                    : input.Equals("CompoundInterest") ? calculator.SubmitOperationInputAndGetResult(compoundInterestOperation())
                    : input.Equals("=") ? calculator.SubmitEqualsRequestAndGetResult()
                    : calculator.SubmitValueInputAndGetResult(decimal.Parse(input));
            //Mocked operations
            IOperation additionOperation()
                => GetOperation(numberOfOperands: 2, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands) 
                    => operands.ToList()[0] + operands.ToList()[1]));
            IOperation multiplicationOperation()
                => GetOperation(numberOfOperands: 2, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands) 
                    => operands.ToList()[0] * operands.ToList()[1]));
            IOperation squareOperation()
                => GetOperation(numberOfOperands: 1, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands) 
                    => operands.ToList()[0] * operands.ToList()[0]));
            IOperation piOperation()
                => GetOperation(numberOfOperands: 0, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands) 
                    => 3.14M));
            IOperation compoundInterestOperation()
                => GetOperation(numberOfOperands: 3, resultFunction: new Func<IEnumerable<decimal>, decimal>((operands)
                    => operands.ToList()[0] * (decimal)Math.Pow((double)(1 + operands.ToList()[1]), (double)operands.ToList()[2])));
        }
    }
}