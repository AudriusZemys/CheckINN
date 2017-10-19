using System.Collections;
using System.Collections.Generic;
using CheckINN.Domain.Entities;
using Ploeh.AutoFixture;

namespace CheckINN.Domain.Cache
{
    public class DummyCheckCache : ICheckCache
    {
        private readonly List<Check> _cache;

        public DummyCheckCache()
        {
            var fixture = new Fixture().Customize(new MultipleCustomization());
            _cache = fixture.Create<List<Check>>();
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