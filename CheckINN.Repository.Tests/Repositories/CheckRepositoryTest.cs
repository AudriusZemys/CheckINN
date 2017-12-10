using System.Linq;
using CheckINN.Repository.Contexts;
using CheckINN.Repository.Entities;
using CheckINN.Repository.Repositories;
using CheckINN.Repository.Tests.Contexts;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using static NUnit.Framework.Assert;

namespace CheckINN.Repository.Tests.Repositories
{
    [TestFixture]
    public class CheckRepositoryTest
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
            var mockContext = new Mock<CheckINNContext>();
            var repo = new CheckRepository(() =>
            {
                var set = new FakeDbSet<Check>();
                mockContext.Setup(context => context.Checks).Returns(set);
                return mockContext.Object;
            });
            var check = _fixture.Create<Check>();

            // act
            repo.Save(check);

            // assert
            mockContext.Verify(context => context.Checks);
            AreEqual(mockContext.Object.Checks.First().CheckId, check.CheckId);
            AreEqual(mockContext.Object.Checks.First().Date, check.Date);

        }
    }
}
