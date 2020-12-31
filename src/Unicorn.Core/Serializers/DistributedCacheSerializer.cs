using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Serializers
{
    public class DistributedCacheSerializer : IDistributedCacheSerializer
    {
        private IJsonSerializer _serializer;
        public DistributedCacheSerializer(IJsonSerializer serializer)
        {
            _serializer = serializer;
        }
        public T Deserialize<T>(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return _serializer.Deserialize<T>(json);
        }

        public byte[] Serialize<T>(T obj)
        {
            var json = _serializer.Serialize(obj);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
