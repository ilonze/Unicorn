using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Exceptions
{
    public class RouteValueMissingException : Exception
    {
        public string RouteName { get; }
        public RouteValueMissingException(string routeName) : base("missing the route value \"" + routeName + "\"")
        {
            RouteName = routeName;
        }
    }
}
