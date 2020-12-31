using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Unicorn.Core.Serializers.Newtonsoft
{
    public class JsonIsoDateTimeConverter : IsoDateTimeConverter
    {
        private IClock _clock;
        public JsonIsoDateTimeConverter(JsonSerializerOptions options)
        {
            _clock = new Clock(options.Kind);

            if (options.DefaultDateTimeFormat != null)
            {
                DateTimeFormat = options.DefaultDateTimeFormat;
            }
        }
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            NewtonsoftJsonSerializer serializer)
        {
            var date = base.ReadJson(reader, objectType, existingValue, serializer) as DateTime?;

            if (date.HasValue)
            {
                return _clock.Normalize(date.Value);
            }

            return null;
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            NewtonsoftJsonSerializer serializer)
        {
            var date = value as DateTime?;
            base.WriteJson(writer, date.HasValue ? _clock.Normalize(date.Value) : value, serializer);
        }
    }
}
