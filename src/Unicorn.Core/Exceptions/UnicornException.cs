using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Exceptions
{
    public class UnicornException: Exception
    {
        public UnicornException():base()
        {

        }
        public UnicornException(string? message):base(message)
        {

        }
        public UnicornException(string? message, Exception? innerException):base(message, innerException)
        {

        }
    }
}
