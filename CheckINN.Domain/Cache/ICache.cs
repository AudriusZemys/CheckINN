using System.Collections.Generic;

namespace CheckINN.Domain.Cache
{
    /// <summary>
    /// Generic iteratable cache interface
    /// </summary>
    /// <typeparam name="T">The item that is stored</typeparam>
    public interface ICache<T> : IEnumerable<T>
    {
        /// <summary>
        /// Put's the cache item into cache
        /// </summary>
        /// <param name="item">Item that has to be cached</param>
        void Put(T item);
    }
}
