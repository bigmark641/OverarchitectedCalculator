using System;
using System.Collections.Immutable;
using Xunit;
using Moq;
using FluentAssertions;
using Calculator.CalculatorEngine;
using Calculator.CalculatorEngine.Implementations;

namespace Calculator.Tests.CalculatorEngine
{
    public class CalculatorStateFactoryTests
    {
        CalculatorStateFactory GetCalculatorStateFactory()
                => new CalculatorStateFactory();

        [Fact]
        public void Constructor_Always_ReturnsCalculatorStateWithValues()
        {
            //Assert
            constructedCalculatorStateValues().Should().BeEquivalentTo(valuesForConstructor());

            //Constructed calculator state values
            IImmutableList<decimal> constructedCalculatorStateValues()
                => constructedCalculatorState().Values;
            ICalculatorState constructedCalculatorState()
                => GetCalculatorStateFactory().GetCalculatorState(valuesForConstructor(), null);
            
            //Values for constructor
            IImmutableList<decimal> valuesForConstructor()
                => ImmutableList<decimal>.Empty.Add(123);
        }

        [Fact]
        public void Constructor_Always_ReturnsCalculatorStateWithOperation()
        {
            //Arrange
            IOperation operationForConstructor = Mock.Of<IOperation>();

            //Assert
            constructedCalculatorStateOperation().Should().BeSameAs(operationForConstructor);

            //Constructed calculator state operation
            IOperation constructedCalculatorStateOperation()
                => constructedCalculatorState().ActiveOperation;
            ICalculatorState constructedCalculatorState()
                => GetCalculatorStateFactory().GetCalculatorState(null, operationForConstructor);
        }
    }
}