using System.Collections.Generic;
using CheckINN.Domain.Entities;

namespace CheckINN.Domain.Parser
{
    public interface IShopParser
    {
        IEnumerable<Product> ParseProductList(string text);
    }
}
