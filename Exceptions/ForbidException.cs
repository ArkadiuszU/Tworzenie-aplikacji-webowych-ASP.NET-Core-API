using System;
using System.Runtime.Serialization;

namespace WebApplication1.Expression
{
    public class ForbidException : Exception
    {
        public ForbidException(string message) : base(message)
        {
        }
    }
}