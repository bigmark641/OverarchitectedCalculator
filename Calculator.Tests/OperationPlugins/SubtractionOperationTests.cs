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
    public class SubtractionOperationTests : OperationTests
    {

        [Fact]
        public void GetNumberOfOperands_Always_ReturnsTwo()
        {
            //Assert
            numberOfOperands().Should().Be(2);

            //Local functions
            int numberOfOperands()
                => new SubtractionOperation().NumberOfOperands();
        }

        [Theory]
        [InlineData(.5, 1.5, 1)]
        [InlineData(1, 1, 0)]
        [InlineData(3, 1, -2)]
        [InlineData(0, 0, 0)]
        public void GetResultForOperands_Operands_Results(decimal expectedResult, params object[] operands)
        {
            //Assert
            resultForOperands().Should().Be(expectedResult);

            //Local functions
            decimal resultForOperands()
                => new SubtractionOperation().ResultForOperands(DecimalOperands(operands));
        }
    }
}