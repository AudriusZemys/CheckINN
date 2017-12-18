using System.Collections.Generic;

namespace CheckINN.Domain.Entities
{
    public class Check
    {
        public Check(ShopIdentifier shop, string shopAddress, IEnumerable<Product> products)
        {
            Shop = shop;
            ShopAddress = shopAddress;
            Products = products;
        }

        public ShopIdentifier Shop { get; }
        public string ShopAddress { get; }
        public IEnumerable<Product> Products { get; }
    }
}
