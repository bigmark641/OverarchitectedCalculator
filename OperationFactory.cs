using System;
using System.Linq;
using TextCalculator;
using Calculator;
using Calculator.Implementations;
using Calculator.Implementations.Operations;

namespace Calculator
{
    class OperationFactory : IOperationFactory
    {
        public IOperation CreateOperation(string operatorSymbol)
        {
            return CreateObjectFromDefaultConstructor<IOperation>(x => TypeHasOperatorSymbol(x, operatorSymbol));
        }

        private bool TypeHasOperatorSymbol(Type type, string operatorSymbol)
        {
            var operatorAttribute = GetOperatorAttribute(type);
            return operatorAttribute != null
                ? operatorAttribute.Symbol.Equals(operatorSymbol)
                : false;
        }

        private OperatorAttribute GetOperatorAttribute(Type type)
        {
            return (OperatorAttribute)Attribute.GetCustomAttribute(type, typeof(OperatorAttribute));
        }

        private T CreateObjectFromDefaultConstructor<T>(Func<Type, bool> typeFilter)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(x => x.GetTypes());
            var assignableTypes = types.Where(x => typeof(T).IsAssignableFrom(x) && typeFilter(x));
            var filteredTypes = assignableTypes.Where(typeFilter);
            var defaultConstructors = filteredTypes.Select(x => x.GetConstructor(Type.EmptyTypes));
            return (T)defaultConstructors.Single().Invoke(new object[]{});           
        }
    }
}