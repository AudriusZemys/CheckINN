using System.Linq;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Entities;
using CheckINN.WebApi.Controllers;
using NUnit.Framework;
using Ploeh.AutoFixture;
using static NUnit.Framework.CollectionAssert;

namespace CheckINN.WebApi.Tests.Controllers
{
    [TestFixture]
    public class CacheControllerTest
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void GetAllProducts_ReturnsProperList()
        {
            // arrange
            var check = _fixture.Create<Check>();
            var cache = new CheckCache();
            cache.Put(check);
            var controller = new CacheController(cache);

            // act
            var result = controller.GetAllProducts();

            // assert
            var expected = check.CheckBody.Products;
            AreEquivalent(expected, result);
        }
    }
}
