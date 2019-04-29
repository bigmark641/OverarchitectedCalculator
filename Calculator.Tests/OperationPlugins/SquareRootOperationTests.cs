using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using Calculator.OperationPlugins;
using Calculator.Utilities;

namespace Calculator.Tests.OperationPlugins
{
    public class SquareRootOperationTests : OperationTests
    {

        [Fact]
        public void GetNumberOfOperands_Always_ReturnsOne()
        {
            //Assert
            numberOfOperands().Should().Be(1);

            //Local functions
            int numberOfOperands()
                => new SquareRootOperation().NumberOfOperands();
        }

        [Theory]
        [InlineData(3, 9)]
        [InlineData(1, 1)]
        [InlineData(0, 0)]
        public void GetResultForOperands_Operands_Results(decimal expectedResult, params object[] operands)
        {
            //Assert
            resultForOperands().Should().Be(Validated(expectedResult));

            //Local functions
            Validated<decimal> resultForOperands()
                => new SquareRootOperation().ResultForOperands(DecimalOperands(operands));
        }

        [Fact]
        public void GetResultForOperands_NegativeOperand_ThrowsArgumentOutOfRangeException()
        {
            //Assert
            resultForNegativeOperand().Should().Be(expectedErrorForNegativeOperand());

            //Local functions
            Validated<decimal> resultForNegativeOperand()
                => new SquareRootOperation().ResultForOperands(DecimalOperands(negativeOperand()));
            IEnumerable<object> negativeOperand()
                => new List<object> { -1 };
            Validated<decimal> expectedErrorForNegativeOperand()
                => new StringError("Only positive numbers can be square rooted.");
        }
    }
}