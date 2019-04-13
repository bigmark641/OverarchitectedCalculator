using System;

namespace Calculator.OperationPlugins
{
    public class OperatorAttribute : Attribute
    {
        public string Symbol {get;}

        public OperatorAttribute(string symbol)
        {
            Symbol = symbol;
        }
    }
}