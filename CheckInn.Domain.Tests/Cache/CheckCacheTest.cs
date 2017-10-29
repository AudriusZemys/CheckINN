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
            AreEquivalent(data.CheckBody.Products, result.CheckBody.Products);
            AreEqualIgnoringCase(data.CheckFooter.CashRegister, result.CheckFooter.CashRegister);
            AreEqualIgnoringCase(data.CheckHeader.ShopIdentifierString, result.CheckHeader.ShopIdentifierString);
            Assert.AreEqual(data.CheckHeader.ShopIdentifier, result.CheckHeader.ShopIdentifier);
        }
    }
}
