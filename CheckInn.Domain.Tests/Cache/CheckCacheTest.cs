using System.Linq;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Entities;
using NUnit.Framework;
using Ploeh.AutoFixture;
using static NUnit.Framework.CollectionAssert;
using static NUnit.Framework.StringAssert;

namespace CheckInn.Domain.Tests.Cache
{
    [TestFixture]
    public class CheckCacheTest
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void Put_PutsItemsToCache()
        {
            // arrange
            var cache = new CheckCache();
            var data = _fixture.Create<Check>();

            // act
            cache.Put(data);

            // assert
            var result = cache.First();
            AreEquivalent(data.Products, result.Products);
            AreEqualIgnoringCase(data.ShopAddress, result.ShopAddress);
            Assert.AreEqual(data.Shop, result.Shop);
        }
    }
}
