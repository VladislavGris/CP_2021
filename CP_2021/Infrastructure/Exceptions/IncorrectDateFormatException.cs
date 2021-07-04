using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Exceptions
{
    class IncorrectDateFormatException : Exception
    {
        public IncorrectDateFormatException() : base() { }
        public IncorrectDateFormatException(string message) : base(message) { }
        public IncorrectDateFormatException(string message, Exception inner) : base(message, inner) { }
    }
}
