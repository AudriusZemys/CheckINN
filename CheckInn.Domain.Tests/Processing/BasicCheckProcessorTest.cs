using System.Collections;
using System.Collections.Generic;
using CheckINN.Domain.Processing;
using CheckINN.Repository.Entities;
using CheckINN.Repository.Repositories;
using log4net;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Check = CheckINN.Domain.Entities.Check;

namespace CheckInn.Domain.Tests.Processing
{
    [TestFixture]
    public class BasicCheckProcessorTest
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void TryProcess_VerifyRepositoryInvoked()
        {
            // arrange
            var productRepoMock = new Mock<ProductListingRepository>();
            var shopRepoMock = new Mock<ShopRepository>();
            Shop shop = null;
            shopRepoMock.Setup(repository => repository.TryGetByAddress(It.IsAny<string>(), ref shop)).Returns(true);
            var logMock = new Mock<ILog>();
            var processor = new BasicCheckProcessor(productRepoMock.Object, logMock.Object, shopRepoMock.Object);
            var check = _fixture.Create<Check>();

            // act
            processor.TryProcess(check);

            // assert
            productRepoMock.Verify(repository => repository.SaveMany(It.IsAny<IEnumerable<ProductListing>>()));
        }
    }
}
