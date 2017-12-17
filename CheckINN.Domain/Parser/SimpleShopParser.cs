using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CheckINN.Domain.Entities;
using static System.String;

namespace CheckINN.Domain.Parser
{
    public class SimpleShopParser : IShopParser
    {
        private readonly Random _random;

        public SimpleShopParser()
        {
            _random = new Random();
        }

        public IEnumerable<Product> ParseProductList(string text)
        {
            var regex = new Regex(@"(.+)\n", RegexOptions.Multiline);
            var matches = regex.Matches(text);
            if (matches.Count < 1)
            {
                throw new Exception("Not matched");
            }

            foreach (Group matchGroup in matches)
            {
                var line = matchGroup.Value.TrimEnd('\n');
                if (!IsNullOrEmpty(line))
                {
                    var rValue = new decimal(_random.NextDouble() * 50);
                    yield return new Product(line, rValue);
                }
            }
        }
    }
}