using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Exceptions
{
    class IncorrectSearchValueException : Exception
    {
        public IncorrectSearchValueException() : base() { }
        public IncorrectSearchValueException(string message) : base(message) { }
        public IncorrectSearchValueException(string message, Exception inner) : base(message, inner) { }
    }
}
