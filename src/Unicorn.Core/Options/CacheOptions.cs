namespace Unicorn.Options
{
    public class CacheOptions
    {
        public bool IsEnabled { get; set; } = false;

        public int TtlSeconds { get; set; } = 60 * 5;

        //public string Region { get; set; }
    }
}
