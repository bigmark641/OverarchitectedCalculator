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
    public class AdditionOperationTests : OperationTests
    {

        [Fact]
        public void GetNumberOfOperands_Always_ReturnsTwo()
        {
            //Assert
            numberOfOperands().Should().Be(2);

            //Numer of operands
            int numberOfOperands()
                => new AdditionOperation().GetNumberOfOperands();
        }

        [Theory]
        [InlineData(2,1,1)]
        [InlineData(1,1,0)]
        [InlineData(-1,1,-2)]
        [InlineData(0,0,0)]
        public void GetResultForOperands_Operands_Results(decimal expectedResult, params object[] operands)
        {
            //Assert
            resultForOperands().Should().Be(expectedResult);

            //Results for operands
            decimal resultForOperands()
                => new AdditionOperation().GetResultForOperands(DecimalOperands(operands));
        }
    }
}