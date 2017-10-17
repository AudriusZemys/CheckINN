using System.Collections;
using System.Collections.Generic;

namespace CheckINN.Domain.Entities
{
    public class CheckBody
    {
        public IEnumerable<Product> Products { get; }

        public CheckBody(IEnumerable<Product> products)
        {
            Products = products;
        }
    }
}