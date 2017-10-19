using System;
using System.Collections;
using System.Collections.Generic;
using CheckINN.Domain.Entities;

namespace CheckINN.Domain.Cache
{
    public class CheckCache : ICheckCache
    {
        private readonly List<Check> _cache;

        public CheckCache()
        {
            _cache = new List<Check>();
        }

        public IEnumerator<Check> GetEnumerator()
        {
            return _cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Put(Check item)
        {
            _cache.Add(item);
        }
    }
}
