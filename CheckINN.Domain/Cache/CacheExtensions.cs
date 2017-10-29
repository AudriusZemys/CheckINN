using System.Linq;

namespace CheckINN.Domain.Cache
{
    public static class CacheExtensions
    {
        public static void CacheClear(this ICheckCache cache)
        {
            if (cache.Any())
                cache = new CheckCache();
        }
    }
}
