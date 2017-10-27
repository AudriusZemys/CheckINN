using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CheckINN.Domain.Entities;

namespace CheckINN.Domain.Parser
{
    public class SimpleShopParser : IShopParser
    {
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
                yield return new Product(matchGroup.Value.TrimEnd('\n'), 0);
            }
        }
    }
}