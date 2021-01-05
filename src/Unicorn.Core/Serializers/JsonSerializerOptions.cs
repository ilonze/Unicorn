using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Serializers
{
    public class JsonSerializerOptions
    {
        /// <summary>
        /// Default: <see cref="DateTimeKind.Unspecified"/>
        /// </summary>
        public DateTimeKind Kind { get; set; }

        public string DefaultDateTimeFormat { get; set; }
    }
}
