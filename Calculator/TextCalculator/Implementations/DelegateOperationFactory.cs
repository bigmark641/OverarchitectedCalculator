using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Calculator.TextCalculator;
using Calculator.CalculatorEngine;

namespace Calculator.TextCalculator.Implementations
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