namespace Unicorn.Options
{
    public class CacheOptions : IOptions
    {
        public bool IsEnabled { get; set; } = false;

        public int TtlSeconds { get; set; } = 60 * 5;

        //public string Region { get; set; }
    }
}
