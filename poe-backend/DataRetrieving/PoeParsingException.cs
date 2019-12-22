using System;

namespace poe_backend.DataRetrieving
{
    public class PoeParsingException : Exception
    {
        
        public PoeParsingException(string message) : base(message)
        {
        }
        
        public PoeParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}