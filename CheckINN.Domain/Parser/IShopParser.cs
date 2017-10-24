using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CheckINN.Domain.Entities;

namespace CheckINN.Domain.Parser
{
    public interface IShopParser
    {
        IEnumerable<Product> ParseProductList(string text);
    }

    public class SimpleShopParser : IShopParser
    {
        public IEnumerable<Product> ParseProductList(string text)
        {
            var regex = new Regex(@"(.+)\n");
            var match = regex.Match(text);
            if (!match.Success)
            {
                throw new Exception("Not matched");
            }

            foreach (Group matchGroup in match.Groups)
            {
                yield return new Product(matchGroup.Value, 0);
            }
        }
    }
}
