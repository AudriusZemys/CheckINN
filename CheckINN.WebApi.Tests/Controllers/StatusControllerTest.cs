using CheckINN.WebApi.Controllers;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using static NUnit.Framework.StringAssert;

namespace CheckINN.WebApi.Tests.Controllers
{
    [TestFixture]
    public class StatusControllerTest
    {
        [Test]
        public void GetStatus_AlwaysReturnsOK()
        {
            // arrange
            var controller = new StatusController();

            // act
            var result = controller.GetStatus();

            // assert
            NotNull(result);
            AreEqual(true, result.Success);
            AreEqualIgnoringCase("OK", result.Message);
        }
    }
}
