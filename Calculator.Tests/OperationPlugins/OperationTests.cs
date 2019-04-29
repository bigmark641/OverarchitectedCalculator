using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using Calculator.CalculatorEngine;
using Calculator.OperationPlugins;
using Calculator.Utilities;

namespace Calculator.Tests.OperationPlugins
{
    public class OperationTests
    {

        protected IEnumerable<decimal> DecimalOperands(IEnumerable<object> operands)
        {
            return operands.Select(x => asDecimal(x));

            //Local functions
            decimal asDecimal(object number)
            => number is int ?
                (decimal) (int) number :
                (decimal) (double) number;
        }

        protected Validated<decimal> Validated(decimal raw)
            => new Validated<decimal>(raw);

        [Fact]
        public void GetResultForValidatedOperands_TernaryOperationWithThreeOperands_ReturnsResult()
        {
            //Assert
            resultOfTernaryOperationWithThreeOperands().Should().Be(TernaryOperationMock.RESULT_OF_OPERATION);

            //Result of operation
            Validated<decimal> resultOfTernaryOperationWithThreeOperands()
                => TernaryOperation().ResultForOperands(threeOperands());
            IEnumerable<decimal> threeOperands()
                => new List<decimal> { 1, 1, 1 };
        }
        
        IOperation TernaryOperation() 
            => new TernaryOperationMock();

        class TernaryOperationMock : Operation
        {
            public static Validated<decimal> RESULT_OF_OPERATION = new Validated<decimal>(123);

            public override int NumberOfOperands()
                => 3;

            protected override Validated<decimal> ResultForValidatedOperands(IList<decimal> operands)
                => RESULT_OF_OPERATION;
        }

        [Fact]
        public void GetResultForValidatedOperands_TernaryOperationWithTwoOperands_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<ArgumentException>(() => resultOfTernaryOperationWithTwoOperands());
            
            //Local functions
            Validated<decimal> resultOfTernaryOperationWithTwoOperands()
                => TernaryOperation().ResultForOperands(twoOperands());
            IEnumerable<decimal> twoOperands()
                => new List<decimal> { 1, 1 };
        }

        [Fact]
        public void GetResultForValidatedOperands_TernaryOperationWithFourOperands_ThrowsInvalidOperationException()
        {
            //Assert
            Assert.Throws<ArgumentException>(() => resultOfTernaryOperationWithTwoOperands());
            
            //Local functions
            Validated<decimal> resultOfTernaryOperationWithTwoOperands()
                => TernaryOperation().ResultForOperands(fourOperands());
            IEnumerable<decimal> fourOperands()
                => new List<decimal> { 1, 1, 1, 1 };
        }
    }
}