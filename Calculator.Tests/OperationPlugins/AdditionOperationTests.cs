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
    public class AdditionOperationTests : OperationTests
    {

        [Fact]
        public void GetNumberOfOperands_Always_ReturnsTwo()
        {
            //Assert
            numberOfOperands().Should().Be(2);

            //Local functions
            int numberOfOperands()
                => new AdditionOperation().NumberOfOperands();
        }

        [Theory]
        [InlineData(2.5, 1, 1.5)]
        [InlineData(1, 1, 0)]
        [InlineData(-1, 1, -2)]
        [InlineData(0, 0, 0)]
        public void GetResultForOperands_Operands_Results(decimal expectedResult, params object[] operands)
        {
            //Assert
            resultForOperands().Should().Be(Validated(expectedResult));

            //Local functions
            Validated<decimal> resultForOperands()
                => new AdditionOperation().ResultForOperands(DecimalOperands(operands));
        }
    }
}