﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Calculator.CalculatorEngine;
using Calculator.CalculatorEngine.Implementations;
using Calculator.TextCalculator;
using Calculator.TextCalculator.Implementations;
using Calculator.OperationPlugins;

namespace Calculator
{
    class Program
    {
        static readonly ITextCalculator TextCalculator = GetTextCalculator();

        static void Main(string[] args)
        {
            //Simulate user input
            SubmitUserInputAndPrintResultWithDelay("PI");
            SubmitUserInputAndPrintResultWithDelay("+");
            SubmitUserInputAndPrintResultWithDelay("2");
            SubmitUserInputAndPrintResultWithDelay("-");
            SubmitUserInputAndPrintResultWithDelay("3");
            SubmitUserInputAndPrintResultWithDelay("neg");
            SubmitUserInputAndPrintResultWithDelay("^2");
            SubmitUserInputAndPrintResultWithDelay("sqrt");
            SubmitUserInputAndPrintResultWithDelay("CompoundInterest");
            SubmitUserInputAndPrintResultWithDelay(".1");
            SubmitUserInputAndPrintResultWithDelay("2");
            SubmitUserInputAndPrintResultWithDelay("=");
        }

        private static ITextCalculator GetTextCalculator()
        {
            //Get concrete TextCalculator with injected dependencies
            return new TextCalculator.Implementations.TextCalculator(calculator(), operationFactory());

            //Local functions
            ICalculator calculator() => new CalculatorEngine.Implementations.Calculator(calculatorStateFactory());
            ICalculatorStateFactory calculatorStateFactory() => new CalculatorStateFactory();
            IOperationFactory operationFactory() => new DelegateOperationFactory(operationForOperatorSymbol);
            IOperation operationForOperatorSymbol(string operatorSymbol) => (IOperation)defaultConstructorForOperatorSymbol(operatorSymbol).Invoke(new object[]{});  
            ConstructorInfo defaultConstructorForOperatorSymbol(string operatorSymbol) => defaultConstructorsByOperatorSymbol(operatorSymbol).Single();          
            IEnumerable<ConstructorInfo> defaultConstructorsByOperatorSymbol(string operatorSymbol) => typesByOperatorSymbol(operatorSymbol).Select(defaultConstructorForType);
            ConstructorInfo defaultConstructorForType(Type type) => type.GetConstructor(Type.EmptyTypes);
            IEnumerable<Type> typesByOperatorSymbol(string operatorSymbol) => typesAssignableToIOperation().Where(x => typeHasOperatorSymbol(x, operatorSymbol));
            IEnumerable<Type> typesAssignableToIOperation() => types().Where(x => typeof(IOperation).IsAssignableFrom(x));
            IEnumerable<Type> types() => assemblies().SelectMany(x => x.GetTypes());
            Assembly[] assemblies() => AppDomain.CurrentDomain.GetAssemblies();  
            bool typeHasOperatorSymbol(Type type, string operatorSymbol) => operatorAttributeForType(type) != null
                ? operatorAttributeForType(type).Symbol.Equals(operatorSymbol)
                : false;
            OperatorAttribute operatorAttributeForType(Type type) => (OperatorAttribute)Attribute.GetCustomAttribute(type, typeof(OperatorAttribute));
        }

        private static void SubmitUserInputAndPrintResultWithDelay(string input)
        {
            //Print input
            PrintInput(input);
            Delay();
            
            //Print result
            PrintResult(result());
            string result() => TextCalculator.SubmitInputAndGetResult(input);
            Delay();
        }

        private static void PrintInput(string input)
        {
            Console.WriteLine(stringToPrint());
            string stringToPrint() => $"Submitting input: {input}";
        }

        private static void PrintResult(string result)
        {
            Console.WriteLine(stringToPrint());
            string stringToPrint() => $"              Receiving result: {result}";
        }

        private static void Delay()
        {
            System.Threading.Thread.Sleep(MILLISECONDS_TO_DELAY());
            int MILLISECONDS_TO_DELAY() => 500;
        }
    }
}
