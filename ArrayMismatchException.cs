using System;

namespace SF2022UserLib
{
    public class ArrayMismatchException : Exception
    {
        public string? FirstArray { get; private set; }
        public string? SecondArray { get; private set; }
        public ArrayMismatchException() : base() { }
        public ArrayMismatchException(string? message) : base(message) { }
        public ArrayMismatchException(string firstArray, string secondArray) : base()
        {
            FirstArray = firstArray;
            SecondArray = secondArray;
        }
        public ArrayMismatchException(string? message, string? firstArray, string? secondArray) : base(message)
        {
            FirstArray = firstArray;
            SecondArray = secondArray;
        }
    }
}
