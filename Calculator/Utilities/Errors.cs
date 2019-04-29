using System;

namespace Calculator.Utilities
{
    
    public class Error { }

    public class StringError : Error 
    {
        private string ErrorString { get; }

        public StringError(string errorString)
        {
            ErrorString = errorString;
        }

        public override bool Equals(object obj)
        {
            return ErrorString == (obj as StringError).ErrorString;
        }

        public override int GetHashCode()
        {
            return ErrorString.GetHashCode();
        }

        public override string ToString()
            => ErrorString;
    }
}