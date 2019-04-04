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
        private Func<string, IOperation> GetOperationByOperatorSymbolFunc { get; }

        public DelegateOperationFactory(Func<string, IOperation> getOperationByOperatorSymbol)
            => GetOperationByOperatorSymbolFunc = getOperationByOperatorSymbol;

        public IOperation GetOperationByOperatorSymbol(string operatorSymbol)
            => GetOperationByOperatorSymbolFunc(operatorSymbol);
    }
}