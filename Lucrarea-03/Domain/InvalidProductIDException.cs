﻿using System;
using System.Runtime.Serialization;

namespace Exemple.Domain
{
    [Serializable]
    internal class InvalidProductIDException : Exception
    {
        public InvalidProductIDException()
        {
        }

        public InvalidProductIDException(string? message) : base(message)
        {
        }

        public InvalidProductIDException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidProductIDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}