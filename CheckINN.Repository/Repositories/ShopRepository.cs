using System;
using System.Collections.Generic;
using System.Linq;
using CheckINN.Repository.Contexts;
using CheckINN.Repository.Entities;

namespace CheckINN.Repository.Repositories
{
    public class ShopRepository : IRepository<Shop>
    {
        private readonly Func<CheckINNContext> _contextFactory;

        public ShopRepository() {}

        public ShopRepository(Func<CheckINNContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public virtual void Save(Shop item)
        {
            using (var context = _contextFactory.Invoke())
            {
                context.Shops.Add(item);
                context.SaveChanges();
            }
        }

        public virtual void SaveMany(IEnumerable<Shop> items)
        {
            using (var context = _contextFactory.Invoke())
            {
                foreach (var item in items)
                {
                    context.Shops.Add(item);
                }
                context.SaveChanges();
            }
        }

        public virtual IEnumerable<Shop> GetAllExisting()
        {
            using (var context = _contextFactory.Invoke())
            {
                return context.Shops.Select(shop => shop);
            }
        }

        public virtual IEnumerable<string> GetExistingNetworks()
        {
            using (var context = _contextFactory.Invoke())
            {
                return context.Shops.GroupBy(shop => shop.Name).Select(shops => shops.Key);
            }
        }

        public virtual bool TryGetByAddress(string address, ref Shop shop)
        {
            using (var context = _contextFactory.Invoke())
            {
                shop = context.Shops.FirstOrDefault(item => string.Equals(address, item.Address));
                if (shop != null) return true;
            }
            return false;
        }
    }
}