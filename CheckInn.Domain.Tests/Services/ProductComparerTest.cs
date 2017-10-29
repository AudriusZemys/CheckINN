using CheckINN.Domain.Entities;
using CheckINN.Domain.Services;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace CheckInn.Domain.Tests.Services
{
    [TestFixture]
    public class ProductComparerTest
    {
        [TestCase("preke", "preke", false)]
        [TestCase("preke", "prekes", true)]
        [TestCase("", "preke", true)]
        [TestCase("preke123", "preke", true)]
        [TestCase("preke", "PReke", true)]
        [TestCase("PReke", "PReke", false)]
        [TestCase(null, "PReke", true)]
        [TestCase("PReke", null, true)]
        public void Compare_ComparesAllCasesProperly(string productA, string productB, bool expectFail)
        {
            // arrange
            var comparer = new ProductComparer();
            var productObjectA = new Product(productA, 0);
            var productObjectB = new Product(productB, 0);

            // act
            var result = comparer.Compare(productObjectA, productObjectB);

            // assert

            if (expectFail)
            {
                NotZero(result);
            }
            else
            {
                Zero(result);
            }

        }
    }
}
