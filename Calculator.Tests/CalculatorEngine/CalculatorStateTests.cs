using System;
using System.Collections.Immutable;
using Xunit;
using Moq;
using FluentAssertions;
using Calculator.CalculatorEngine;
using Calculator.CalculatorEngine.Implementations;

namespace Calculator.Tests.CalculatorEngine
{
    public class CalculatorStateTests
    {
    
        [Fact]
        public void Constructor_Always_ConstructsStateWithValues()
        {
            //Assert
            constructedCalculatorStateValues().Should().BeEquivalentTo(valuesForConstructor());

            //Local functions
            IImmutableList<decimal> constructedCalculatorStateValues()
                => constructedCalculatorState().Values;
            CalculatorState constructedCalculatorState() 
                => new CalculatorState(valuesForConstructor(), null);
            IImmutableList<decimal> valuesForConstructor()
                => ImmutableList<decimal>.Empty.Add(123);
        }
    
        [Fact]
        public void Constructor_Always_ConstructsStateWithOperation()
        {
            //Arrange
            IOperation operationForConstructor = Mock.Of<IOperation>();

            //Assert
            constructedCalculatorStateOperation().Should().BeSameAs(operationForConstructor);
            
            //Local functions
            IOperation constructedCalculatorStateOperation()
                => constructedCalculatorState().ActiveOperation;
            CalculatorState constructedCalculatorState() 
                => new CalculatorState(null, operationForConstructor);
        }
    }
}