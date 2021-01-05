using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Serializers.Newtonsoft
{
    public interface IClock
    {
        DateTime Now { get; }

        DateTimeKind Kind { get; }

        bool SupportsMultipleTimezone { get; }

        DateTime Normalize(DateTime dateTime);
    }
}
