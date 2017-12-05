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
            var checkRepoMock = new Mock<CheckRepository>();
            var productRepoMock = new Mock<ProductListingRepository>();
            var processor = new BasicCheckProcessor(checkRepoMock.Object, productRepoMock.Object);
            var check = _fixture.Create<Check>();

            // act
            processor.TryProcess(check);

            // assert
            checkRepoMock.Verify(repository => repository.Save(It.IsAny<CheckINN.Repository.Entities.Check>()));
            productRepoMock.Verify(repository => repository.SaveMany(It.IsAny<IEnumerable<CheckINN.Repository.Entities.ProductListing>>()));
        }
    }
}
