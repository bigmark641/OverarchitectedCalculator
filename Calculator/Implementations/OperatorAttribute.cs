using System;

namespace Calculator.Implementations
{
    class OperatorAttribute : Attribute
    {
        public string Symbol {get;}

        public OperatorAttribute(string symbol)
        {
            Symbol = symbol;
        }
    }
}