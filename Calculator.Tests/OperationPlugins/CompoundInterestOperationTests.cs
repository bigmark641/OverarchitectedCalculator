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
    public class CompoundInterestOperationTests : OperationTests
    {

        [Fact]
        public void GetNumberOfOperands_Always_ReturnsThree()
        {
            //Assert
            numberOfOperands().Should().Be(3);

            //Local functions
            int numberOfOperands()
                => new CompoundInterestOperation().NumberOfOperands();
        }

        [Theory]
        [InlineData(1.21, 1, .1, 2)]
        [InlineData(0, 0, .1, 2)]
        [InlineData(1.234, 1.234, .56, 0)]
        [InlineData(1.234, 1.234, 0, 56)]
        [InlineData(1, 1.21, .1, -2)]        
        [InlineData(.9, 1, -.1, 1)]
        public void GetResultForOperands_Operands_Results(decimal expectedResult, params object[] operands)
        {
            //Assert
            resultForOperands().Should().Be(Validated(expectedResult));

            //Local functions
            Validated<decimal> resultForOperands()
                => new CompoundInterestOperation().ResultForOperands(DecimalOperands(operands));
        }
    }
}