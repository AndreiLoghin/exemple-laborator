using System;
using System.Runtime.Serialization;

namespace LAB1_PSSC_LA.Domain
{
        [Serializable]
        internal class ProductIDException : Exception
        {
            public ProductIDException()
            {
            }

            public ProductIDException(string? message) : base(message)
            {
            }

            public ProductIDException(string? message, Exception? innerException) : base(message, innerException)
            {
            }

            protected ProductIDException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
}
