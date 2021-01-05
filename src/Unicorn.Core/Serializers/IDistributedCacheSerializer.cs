using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Serializers
{
    public interface IDistributedCacheSerializer
    {
        T Deserialize<T>(byte[] bytes);
        byte[] Serialize<T>(T obj);
    }
}
