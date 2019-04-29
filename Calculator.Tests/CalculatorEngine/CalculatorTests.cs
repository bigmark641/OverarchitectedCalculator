using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using FluentAssertions;
using Calculator.CalculatorEngine;
using Calculator.CalculatorEngine.Implementations;
using Calculator.Utilities;

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

        [Theory]  //Format: (expectedDecimalResult, expectedErrorResult, inputs[])
        [InlineData(1,    null, "1")] //Single value
        [InlineData(1,    null, "1", "+")] //Value and operation
        [InlineData(2,    null, "1", "+", "2")] //Last value
        [InlineData(3,    null, "1", "+", "2", "=")] //Equals
        [InlineData(3,    null, "1", "+", "2", "+")] //Previous operation result
        [InlineData(6,    null, "1", "+", "2", "+", "3", "=")] //Chained operations      
        [InlineData(2,    null, "5", "+", "3", "/", "4", "=")] //Chained operations ordered    
        [InlineData(6,    null, "6", "/", "3", "+", "4", "=")] //Chained operations order reversed
        [InlineData(-1,   null, "1", "+", "-2", "=")] //Negatives
        [InlineData(3,    null, "9", "SquareRoot")] //Unary operation
        [InlineData(6,    null, "9", "SquareRoot", "+", "3", "=")] //Unary chained operation
        [InlineData(3,    null, "1", "+", "9", "SquareRoot")] //Unary later operation
        [InlineData(5,    null, "2", "+", "9", "SquareRoot", "=")] //Unary chained operation
        [InlineData(12.1, null, "10", "CompoundInterest", ".1", "2", "=")] //Ternary operation
        [InlineData(3.14, null, "PI")] //Nullary operation
        [InlineData(4.14, null, "PI", "+", "1", "=")] //Nullary chained operation
        [InlineData(5.14, null, "2", "+", "PI", "=")] //Nullary chained operation reversed
        //Invalid operations
        [InlineData(0, "Operation is not currently allowed.",      "+")] //Needs value first
        [InlineData(0, "Equals request is not currently allowed.", "=")] //Needs value first
        [InlineData(0, "Operation is not currently allowed.",      "SquareRoot")] //Missing operand
        [InlineData(0, "Value is not currently allowed.",          "1", "1")] //Needs operation before second value
        [InlineData(0, "Operation is not currently allowed.",      "1", "PI")] //Needs operation before second value
        [InlineData(0, "Equals request is not currently allowed.", "1", "=")] //Missing operation
        [InlineData(0, "Operation is not currently allowed.",      "1", "+", "+")] //Incomplete operation
        [InlineData(0, "Equals request is not currently allowed.", "1", "+", "=")] //Incomplete operation
        //[InlineData(0, "Operation is not currently allowed.",    "1", "+", "SquareRoot")] //Missing operand to SquareRoot
        [InlineData(0, "Value is not currently allowed.",          "1", "+", "1", "1")] //Too many operands
        [InlineData(0, "Operation is not currently allowed.",      "1", "+", "1", "PI")] //Too many operands
        [InlineData(0, "Value is not currently allowed.",          "1", "+", "1", "=", "1")] //Missing second operation
        [InlineData(0, "Operation is not currently allowed.",      "1", "+", "1", "=", "PI")] //Missing second operation
        [InlineData(0, "Equals request is not currently allowed.", "1", "+", "1", "=", "=")] //Missing second operation
        [InlineData(0, "Operation is not currently allowed.",      "1", "CompoundInterest", "1", "+")] //Incomplete operation
        [InlineData(0, "Equals request is not currently allowed.", "1", "CompoundInterest", "1", "=")] //Incomplete operation
        [InlineData(0, "Value is not currently allowed.",          "1", "CompoundInterest", "1", "1", "1")] //Too many operands
        [InlineData(0, "Operation is not currently allowed.",      "1", "CompoundInterest", "1", "1", "PI")] //Too many operands
        [InlineData(0, "Imaginary numbers are not valid.",       "i")] // Nullary operation error
        [InlineData(0, "Cannot square root a negative.",         "-1", "SquareRoot")] // Unary operation error
        [InlineData(0, "Cannot divide by zero.",                   "2", "/", "0", "=")] // Equals operation error
        [InlineData(0, "Cannot divide by zero.",                 "2", "/", "0", "+")] // Chained operation error
        public void TestCalculatorInputResults(decimal expectedDecimalResult, string expectedErrorResult, params string[] inputs)
        {
            //Arrange
            var calculator = new Calculator.CalculatorEngine.Implementations.Calculator(CalculatorStateFactoryMock.Object);
            Validated<decimal> lastValidatedResult = null;

            //Act
            foreach (var input in inputs)
                lastValidatedResult = resultOfSubmittedInput(input);

            //Assert
            lastValidatedResult.Should().Be(expectedValidatedResult());

            //Local functions
            Validated<decimal> resultOfSubmittedInput(string input)
                => input.Equals("+") ? calculator.SubmitOperationInputAndGetResult(additionOperation())
                    : input.Equals("/") ? calculator.SubmitOperationInputAndGetResult(divisionOperation())
                    : input.Equals("SquareRoot") ? calculator.SubmitOperationInputAndGetResult(squareRootOperation())
                    : input.Equals("PI") ? calculator.SubmitOperationInputAndGetResult(piOperation())
                    : input.Equals("i") ? calculator.SubmitOperationInputAndGetResult(iOperation())
                    : input.Equals("CompoundInterest") ? calculator.SubmitOperationInputAndGetResult(compoundInterestOperation())
                    : input.Equals("=") ? calculator.SubmitEqualsRequestAndGetResult()
                    : calculator.SubmitValueInputAndGetResult(decimal.Parse(input));
            IOperation additionOperation()
                => Operation(2, new Func<IEnumerable<decimal>, Validated<decimal>>((operands) 
                    => new Validated<decimal>(operands.ToList()[0] + operands.ToList()[1])));
            IOperation divisionOperation()
                => Operation(2, new Func<IEnumerable<decimal>, Validated<decimal>>((operands) 
                    => operands.ToList()[1] != 0
                        ? new Validated<decimal>(operands.ToList()[0] / operands.ToList()[1])
                        : new StringError("Cannot divide by zero.")));
            IOperation squareRootOperation()
                => Operation(1, new Func<IEnumerable<decimal>, Validated<decimal>>((operands) 
                    => operands.ToList()[0] >= 0
                        ? new Validated<decimal>((decimal) Math.Sqrt((double)operands.ToList()[0]))
                        : new StringError("Cannot square root a negative.")));
            IOperation piOperation()
                => Operation(0, new Func<IEnumerable<decimal>, Validated<decimal>>((operands) 
                    => new Validated<decimal>(3.14M)));
            IOperation iOperation()
                => Operation(0, new Func<IEnumerable<decimal>, Validated<decimal>>((operands) 
                    => new StringError("Imaginary numbers are not valid.")));
            IOperation compoundInterestOperation()
                => Operation(3, new Func<IEnumerable<decimal>, Validated<decimal>>((operands)
                    => new Validated<decimal>(operands.ToList()[0] * (decimal)Math.Pow((double)(1 + operands.ToList()[1]), (double)operands.ToList()[2]))));           
            Validated<decimal> expectedValidatedResult()
                => expectedErrorResult == null
                    ? new Validated<decimal>(expectedDecimalResult)
                    : new StringError(expectedErrorResult);
        }

        private IOperation Operation(int numberOfOperands, Func<IEnumerable<decimal>, Validated<decimal>> resultFunction = null)
        {
            //Setup number of operands
            var operationMock = new Mock<IOperation>();
            operationMock.Setup(x => x.NumberOfOperands()).Returns(numberOfOperands);
            
            //Setup result for operands
            operationMock.Setup(x => x.ResultForOperands(It.IsAny<IEnumerable<decimal>>())).Returns(resultFunction);

            //Return operation
            var operation = operationMock.Object;
            return operation;
        }
    }
}