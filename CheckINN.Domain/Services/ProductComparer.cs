using System.Collections.Generic;
using CheckINN.Domain.Entities;
using static System.String;

namespace CheckINN.Domain.Services
{
    public class ProductComparer : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            return CompareOrdinal(x.ProductEntry, y.ProductEntry);
        }
    }
}
