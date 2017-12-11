using System.Linq;
using CheckINN.Repository.Contexts;
using CheckINN.Repository.Entities;
using CheckINN.Repository.Repositories;
using CheckINN.Repository.Tests.Contexts;
using Moq;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using static NUnit.Framework.StringAssert;

namespace CheckINN.Repository.Tests.Repositories
{
    [TestFixture]
    public class UserRepositoryTest
    {
        [Test]
        public void NewUser_CreatesAUser()
        {
            // arrange
            var mockContext = new Mock<CheckINNContext>();
            var repo = new UserRepository(() => {
                var set = new FakeDbSet<User>();
                mockContext.Setup(context => context.Users).Returns(set);
                return mockContext.Object;
            });

            // act
            repo.NewUser("user", "password");

            // assert
            var rUser = mockContext.Object.Users.First();
            NotNull(rUser);
            AreEqualIgnoringCase("user", rUser.Username);
        }

        [TestCase("password")]
        [TestCase("asdfrwgfwe")]
        [TestCase("g21dbtgf89b")]
        [TestCase("gn./,gtu[pltk9085ty")]
        [TestCase("as")]
        [TestCase("")]
        public void TestCrypto(string password)
        {
            // arrange
            var set = new FakeDbSet<User>();
            var mockContext = new Mock<CheckINNContext>();
            var repo = new UserRepository(() => {
                mockContext.Setup(context => context.Users).Returns(set);
                return mockContext.Object;
            });
            
            // act
            repo.NewUser("username", password);

            // assert
            True(repo.Authenticate("username", password));
        }
    }
}
