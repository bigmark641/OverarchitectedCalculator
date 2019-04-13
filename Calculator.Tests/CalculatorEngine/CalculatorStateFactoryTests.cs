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
        CalculatorStateFactory CalculatorStateFactory()
                => new CalculatorStateFactory();

        [Fact]
        public void Constructor_Always_ReturnsCalculatorStateWithValues()
        {
            //Assert
            constructedCalculatorStateValues().Should().BeEquivalentTo(valuesForConstructor());

            //Local functions
            IImmutableList<decimal> constructedCalculatorStateValues()
                => constructedCalculatorState().Values;
            ICalculatorState constructedCalculatorState()
                => CalculatorStateFactory().NewCalculatorState(valuesForConstructor(), null);
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

            //Local functions
            IOperation constructedCalculatorStateOperation()
                => constructedCalculatorState().ActiveOperation;
            ICalculatorState constructedCalculatorState()
                => CalculatorStateFactory().NewCalculatorState(null, operationForConstructor);
        }
    }
}