using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextCalculator;
using Calculator;

namespace Calculator.Implementations
{
    class DelegateOperationFactory : IOperationFactory
    {
        public Func<string, IOperation> GetOperationByOperatorSymbol { get; }

        public DelegateOperationFactory(Func<string, IOperation> getOperationByOperatorSymbol)
        {
            GetOperationByOperatorSymbol = getOperationByOperatorSymbol;
        }
        
        // (string operatorSymbol)
        //     => GetObjectFromDefaultConstructor<IOperation>(x => TypeHasOperatorSymbol(x, operatorSymbol));

        
    }
}