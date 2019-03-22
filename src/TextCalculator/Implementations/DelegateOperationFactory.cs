using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextCalculator;
using CalculatorEngine;

namespace TextCalculator.Implementations
{
    class DelegateOperationFactory : IOperationFactory
    {
        public Func<string, IOperation> GetOperationByOperatorSymbol { get; }

        public DelegateOperationFactory(Func<string, IOperation> getOperationByOperatorSymbol)
        {
            GetOperationByOperatorSymbol = getOperationByOperatorSymbol;
        }
    }
}