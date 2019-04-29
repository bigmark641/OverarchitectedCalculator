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
    public class NegationOperationTests : OperationTests
    {

        [Fact]
        public void GetNumberOfOperands_Always_ReturnsOne()
        {
            //Assert
            numberOfOperands().Should().Be(1);

            //Local functions
            int numberOfOperands()
                => new NegationOperation().NumberOfOperands();
        }

        [Theory]
        [InlineData(-2, 2)]
        [InlineData(3, -3)]
        [InlineData(0, 0)]
        public void GetResultForOperands_Operands_Results(decimal expectedResult, params object[] operands)
        {
            //Assert
            resultForOperands().Should().Be(Validated(expectedResult));

            //Local functions
            Validated<decimal> resultForOperands()
                => new NegationOperation().ResultForOperands(DecimalOperands(operands));
        }
    }
}