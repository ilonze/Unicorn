using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class JsonToJsonpDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "JsonToJsonp";
        public string Name => ProviderName;

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            if (data is ResponseData)
            {
                var callback = "callback";
                if (context.HttpContext.Request.Query.ContainsKey(callback))
                {
                    callback = context.HttpContext.Request.Query[nameof(callback)];
                }
                data.BodyString = callback + "(" + data.BodyString + ")";
                data.ContentType = ContentTypes.Text.Javascript;
            }
            return Task.FromResult(data);
        }
    }
}
