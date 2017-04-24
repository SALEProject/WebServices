using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.Exceptions
{
    public class InternalException : Exception
    {

        public InternalException()
        {
        }

        public InternalException(string message)
            : base(message)
        {
        }

        public InternalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}