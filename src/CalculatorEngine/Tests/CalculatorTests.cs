using System;
using System.Collections.Immutable;
using System.Collections.Generic;
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
                    calculatorStateMock.Setup(x => x.Operation).Returns(operation);
                    return calculatorStateMock.Object;
                }));
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_OperationWithoutOperandsOnInitialState_ReturnsResult()
        {
            //Assert
            resultOfOperationWithoutOperandsOnInitialState().Should().Be(resultForMock());

            //Result of zero operand operation on initial state
            decimal resultOfOperationWithoutOperandsOnInitialState() 
                => GetCalculator().SubmitOperationInputAndGetResult(operation());
            IOperation operation()
                => GetOperation(operandCount(), resultForMock());
            int operandCount()
                => 0;

            //Result for mock
            decimal resultForMock()
                => 123;
        }

        private IOperation GetOperation(int numberOfOperands, decimal operationResult = 0)
        {
            var operationMock = new Mock<IOperation>();
            operationMock.Setup(x => x.GetNumberOfOperands()).Returns(numberOfOperands);
            operationMock.Setup(x => x.GetResultForOperands(It.IsAny<IList<decimal>>())).Returns(operationResult);
            var operation = operationMock.Object;
            return operation;
        }

        [Fact]
        public void SubmitOperationInputAndGetResult_OperationWithOperandsOnInitialState_ThrowsArgumentException()
        {
            //Assert
            Assert.Throws<ArgumentException>(submitOperationWithOperandsOnInitialState());

            //Result of zero operand operation on initial state
            Action submitOperationWithOperandsOnInitialState()
                => () => GetCalculator().SubmitOperationInputAndGetResult(operation());
            IOperation operation()
                => GetOperation(operandCount());
            int operandCount()
                => 1;
        }

        [Fact]
        public void SubmitValueInputAndGetResult_Always_ReturnsValue()
        {
            //Assert
            resultOfValueInputOnInitialState().Should().Be(valueInput());

            //Result of zero operand operation on initial state
            decimal resultOfValueInputOnInitialState() 
                => GetCalculator().SubmitValueInputAndGetResult(valueInput());

            //Result for mock
            decimal valueInput()
                => 123;            
        }

        private Implementations.Calculator GetCalculator()
        {
            return new Implementations.Calculator(CalculatorStateFactoryMock.Object);
        }
    }
}