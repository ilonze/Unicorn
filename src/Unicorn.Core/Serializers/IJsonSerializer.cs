using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Serializers
{
    public interface IJsonSerializer
    {
        string Serialize(object obj, bool camelCase = true, bool indented = false);

        T Deserialize<T>(string jsonString, bool camelCase = true);

        object Deserialize(Type type, string jsonString, bool camelCase = true);
    }
}
