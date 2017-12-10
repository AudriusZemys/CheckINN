using System.Collections;
using System.Collections.Generic;
using CheckINN.Domain.Entities;
using CheckINN.Domain.Processing;
using CheckINN.Repository.Repositories;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

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
            var processor = new BasicCheckProcessor(productRepoMock.Object);
            var check = _fixture.Create<Check>();

            // act
            processor.TryProcess(check);

            // assert
            productRepoMock.Verify(repository => repository.SaveMany(It.IsAny<IEnumerable<CheckINN.Repository.Entities.ProductListing>>()));
        }
    }
}
