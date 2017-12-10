using System.Linq;
using CheckINN.Repository.Contexts;
using CheckINN.Repository.Entities;
using CheckINN.Repository.Repositories;
using CheckINN.Repository.Tests.Contexts;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using static NUnit.Framework.Assert;
using static NUnit.Framework.StringAssert;

namespace CheckINN.Repository.Tests.Repositories
{
    [TestFixture]
    public class ProductListingRepositoryTest
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void Save_Saves()
        {
            // arrange
            var mockContext = new Mock<ReceiptsContext>();
            var repo = new ProductListingRepository(() =>
            {
                var set = new FakeDbSet<ProductListing>();
                mockContext.Setup(context => context.ProductListings).Returns(set);
                return mockContext.Object;
            });
            var productListing = _fixture.Create<ProductListing>();

            // act
            repo.Save(productListing);

            // assert
            mockContext.Verify(context => context.ProductListings);
            AreEqual(mockContext.Object.ProductListings.First().Check, productListing.Check);
            AreEqual(mockContext.Object.ProductListings.First().Price, productListing.Price);
            AreEqual(mockContext.Object.ProductListings.First().ProductListingId, productListing.ProductListingId);
            AreEqualIgnoringCase(mockContext.Object.ProductListings.First().Name, productListing.Name);
        }

        [Test]
        public void SaveMany_SavesMany()
        {
            // arrange
            var mockContext = new Mock<ReceiptsContext>();
            var repo = new ProductListingRepository(() =>
            {
                var set = new FakeDbSet<ProductListing>();
                mockContext.Setup(context => context.ProductListings).Returns(set);
                return mockContext.Object;
            });
            var productListing = _fixture.CreateMany<ProductListing>();

            // act
            repo.SaveMany(productListing);

            // assert
            CollectionAssert.AreEquivalent(productListing, mockContext.Object.ProductListings.Select(listing => listing));
        }
    }
}
