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
    public class OperatorAttributeTests
    {

        [Fact]
        public void Constructor_Always_SetsSymbol()
        {
            //Assert
            new OperatorAttribute(symbol()).Symbol.Should().Be(symbol());

            //Local functions
            string symbol()
                => "abc";
        }
    }
}