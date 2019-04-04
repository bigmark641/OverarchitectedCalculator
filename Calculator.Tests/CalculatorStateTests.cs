using System;
using System.Collections.Immutable;
using Xunit;
using Moq;
using FluentAssertions;
using CalculatorEngine.Implementations;

namespace CalculatorEngine.Tests
{
    public class CalculatorStateTests
    {
    
        [Fact]
        public void Constructor_Always_ConstructsStateWithValues()
        {
            //Assert
            constructedCalculatorStateValues().Should().BeEquivalentTo(valuesForConstructor());

            //Constructed calculator state values
            IImmutableList<decimal> constructedCalculatorStateValues()
                => constructedCalculatorState().Values;
            CalculatorState constructedCalculatorState() 
                => new CalculatorState(valuesForConstructor(), null);

            //Values for constructor
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
            
            //Constructed calculator state operation
            IOperation constructedCalculatorStateOperation()
                => constructedCalculatorState().ActiveOperation;
            CalculatorState constructedCalculatorState() 
                => new CalculatorState(null, operationForConstructor);
        }
    }
}