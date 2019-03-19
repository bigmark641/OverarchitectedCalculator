using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextCalculator;
using Calculator;
using Calculator.Implementations;
using Calculator.Implementations.Operations;

namespace Calculator
{
    class OperationFactory : IOperationFactory
    {
        public IOperation GetOperation(string operatorSymbol)
            => GetObjectFromDefaultConstructor<IOperation>(x => TypeHasOperatorSymbol(x, operatorSymbol));

        private bool TypeHasOperatorSymbol(Type type, string operatorSymbol)
        {
            return operatorAttribute() != null
                ? operatorAttribute().Symbol.Equals(operatorSymbol)
                : false;
            OperatorAttribute operatorAttribute() 
                => (OperatorAttribute)Attribute.GetCustomAttribute(type, typeof(OperatorAttribute));
        }

        private T GetObjectFromDefaultConstructor<T>(Func<Type, bool> typeFilter)
        {
            return (T)defaultConstructors().Single().Invoke(new object[]{}); 
            IEnumerable<ConstructorInfo> defaultConstructors() => filteredTypes().Select(x => x.GetConstructor(Type.EmptyTypes));
            IEnumerable<Type> filteredTypes() => assignableTypes().Where(typeFilter);
            IEnumerable<Type> assignableTypes() => types().Where(x => typeof(T).IsAssignableFrom(x) && typeFilter(x));
            IEnumerable<Type> types() => assemblies().SelectMany(x => x.GetTypes());
            Assembly[] assemblies() => AppDomain.CurrentDomain.GetAssemblies();          
        }
    }
}