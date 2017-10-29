using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CheckINN.Domain.Entities
{
    public struct CheckBody
    {
        public Product this[int index] => Products.ToList()[index];

        public IEnumerable<Product> Products { get; }

        public CheckBody(IEnumerable<Product> products)
        {
            Products = products;
        }
    }
}