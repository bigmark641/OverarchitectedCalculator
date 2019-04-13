using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using Calculator.OperationPlugins;

namespace Calculator.Tests.OperationPlugins
{
    public class SquareOperationTests : OperationTests
    {

        [Fact]
        public void GetNumberOfOperands_Always_ReturnsOne()
        {
            //Assert
            numberOfOperands().Should().Be(1);

            //Local functions
            int numberOfOperands()
                => new SquareOperation().NumberOfOperands();
        }

        [Theory]
        [InlineData(9, 3)]
        [InlineData(1, 1)]
        [InlineData(0, 0)]
        [InlineData(9, -3)]
        public void GetResultForOperands_Operands_Results(decimal expectedResult, params object[] operands)
        {
            //Assert
            resultForOperands().Should().Be(expectedResult);

            //Local functions
            decimal resultForOperands()
                => new SquareOperation().ResultForOperands(DecimalOperands(operands));
        }
    }
}