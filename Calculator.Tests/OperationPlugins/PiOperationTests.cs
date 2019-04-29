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
    public class PiOperationTests : OperationTests
    {

        [Fact]
        public void GetNumberOfOperands_Always_ReturnsZero()
        {
            //Assert
            numberOfOperands().Should().Be(0);

            //Local functions
            int numberOfOperands()
                => new PiOperation().NumberOfOperands();
        }

        [Fact]
        public void GetResultForOperands_NoOperands_ReturnsPi()
        {
            //Assert
            resultForOperands().Should().Be(Validated(pi()));

            //Local functions
            Validated<decimal> resultForOperands()
                => new PiOperation().ResultForOperands(new List<decimal>());
            decimal pi()
                => (decimal)Math.PI;
        }
    }
}