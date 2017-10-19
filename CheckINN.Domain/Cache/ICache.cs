using System.Collections.Generic;

namespace CheckINN.Domain.Cache
{
    public interface ICache<T> : IEnumerable<T>
    {
        void Put(T item);
    }
}
