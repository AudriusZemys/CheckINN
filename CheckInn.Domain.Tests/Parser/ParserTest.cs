using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Tests.Parser;
using NUnit.Framework;
using static NUnit.Framework.CollectionAssert;

namespace CheckInn.Domain.Tests.Parser
{
    [TestFixture]
    public class ParserTest
    {
        private List<Tuple<string, double>> _expectedResult;

        [SetUp]
        public void Setup()
        {
            _expectedResult = new List<Tuple<string, double>>
            {
                new Tuple<string, double>("Citrinos (5 d.)", 1.39),
                new Tuple<string, double>("Malta kava JACOBS KRONUNG", 4.99),
                new Tuple<string, double>("Kepintos ir sūdytos pistacijos", 14.74),
                new Tuple<string, double>("Brendis J. P. CHENET RESERVE IMPERIALE, 38%", 16.99),
                new Tuple<string, double>("Kepintos saulėgrąžos žM", 1.48),
                new Tuple<string, double>("Tortas JUODOJI ROŽĖ", 6.97)
            };
        }

        [Test]
        public void ParseProductList_OutputsAListOfProducts()
        {
            // arrange
            CheckINN.Domain.Parser.Parser parser = new CheckINN.Domain.Parser.Parser(ContentDelivery.GetContent());

            // act
            parser.Parse();

            // assert
            AreEquivalent(_expectedResult, parser.Products);
        }
    }
}
