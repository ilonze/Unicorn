using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicorn.Core.Exceptions;
using Unicorn.Core.Serializers.Newtonsoft;

namespace Unicorn.Core.Serializers
{
    public class JsonSerializer : IJsonSerializer
    {
        protected JsonSerializerOptions Options { get; }

        protected IServiceScopeFactory ServiceScopeFactory { get; }

        private static readonly CamelCaseExceptDictionaryKeysResolver SharedCamelCaseExceptDictionaryKeysResolver =
            new CamelCaseExceptDictionaryKeysResolver();

        public JsonSerializer(IOptions<JsonSerializerOptions> options, IServiceScopeFactory serviceScopeFactory)
        {
            Options = options.Value;
            ServiceScopeFactory = serviceScopeFactory;
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var settings = CreateSerializerSettings(camelCase);
                return JsonConvert.SerializeObject(obj, settings);
            }
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var settings = CreateSerializerSettings(camelCase);
                return JsonConvert.DeserializeObject<T>(jsonString, settings);
            }
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var settings = CreateSerializerSettings(camelCase);
                return JsonConvert.DeserializeObject(jsonString, type, settings);
            }
        }

        protected virtual JsonSerializerSettings CreateSerializerSettings(bool camelCase = true, bool indented = false)
        {
            var settings = new JsonSerializerSettings();

            settings.Converters.Insert(0, new JsonIsoDateTimeConverter(Options));

            if (camelCase)
            {
                settings.ContractResolver = SharedCamelCaseExceptDictionaryKeysResolver;
            }

            if (indented)
            {
                settings.Formatting = Formatting.Indented;
            }

            return settings;
        }

        private class CamelCaseExceptDictionaryKeysResolver : CamelCasePropertyNamesContractResolver
        {
            protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
            {
                var contract = base.CreateDictionaryContract(objectType);

                contract.DictionaryKeyResolver = propertyName => propertyName;

                return contract;
            }
        }
    }
}
