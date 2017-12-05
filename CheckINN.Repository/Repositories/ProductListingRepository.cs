using System;
using System.Collections.Generic;
using CheckINN.Repository.Contexts;
using CheckINN.Repository.Entities;

namespace CheckINN.Repository.Repositories
{
    public class ProductListingRepository : IRepository<ProductListing>
    {
        private readonly Func<ReceiptsContext> _contextFactory;

        public ProductListingRepository() {}

        public ProductListingRepository(Func<ReceiptsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public virtual void Save(ProductListing productListing)
        {
            using (var context = _contextFactory.Invoke())
            {
                context.ProductListings.Add(productListing);
                context.SaveChanges();
            }
        }

        public virtual void SaveMany(IEnumerable<ProductListing> items)
        {
            using (var context = _contextFactory.Invoke())
            {
                foreach (var item in items)
                {
                    context.ProductListings.Add(item);
                }
                context.SaveChanges();
            }
        }
    }
}