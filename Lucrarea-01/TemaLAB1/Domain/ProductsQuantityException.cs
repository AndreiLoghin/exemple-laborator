using System;
using System.Runtime.Serialization;

namespace LAB1_PSSC_LA.Domain
{
    [Serializable]
    internal class ProductsQuantityException : Exception
    {
        public ProductsQuantityException()
        {
        }

        public ProductsQuantityException(string? message) : base(message)
        {
        }

        public ProductsQuantityException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ProductsQuantityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}