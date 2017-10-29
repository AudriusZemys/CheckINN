using System.Linq;
using CheckINN.Domain.Parser;
using NUnit.Framework;
using static NUnit.Framework.CollectionAssert;

namespace CheckInn.Domain.Tests.Parser
{
    [TestFixture]
    public class SimpleShopParserTest
    {
        private const string Text = "Bread\nEggs\nSome yoghurt\nCookies\n";
        private readonly string[] _expectedResult =
        {
            "Bread",
            "Eggs",
            "Some yoghurt",
            "Cookies"
        };

        [Test]
        public void ParseProductList_OutputsAListOfProducts()
        {
            // arrange
            SimpleShopParser parser = new SimpleShopParser();

            // act
            var result = parser.ParseProductList(Text);

            // assert
            AreEquivalent(_expectedResult, result.Select(product => product.ProductEntry));
        }
    }
}
