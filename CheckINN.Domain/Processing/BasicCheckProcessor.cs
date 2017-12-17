using System;
using System.Linq;
using CheckINN.Domain.Entities;
using CheckINN.Repository.Entities;
using CheckINN.Repository.Repositories;
using log4net;
using Check = CheckINN.Domain.Entities.Check;

namespace CheckINN.Domain.Processing
{
    public class BasicCheckProcessor : ICheckProcessor
    {
        private readonly IRepository<ProductListing> _listingRepo;
        private readonly ShopRepository _shopRepository;
        private readonly ILog _log;

        public BasicCheckProcessor(IRepository<ProductListing> listingRepo, ILog log, ShopRepository shopRepository)
        {
            _listingRepo = listingRepo;
            _log = log;
            _shopRepository = shopRepository;
        }

        public bool TryProcess(Check item)
        {
            var result = true;
            try
            {
                Shop shop = null;
                if (!_shopRepository.TryGetByAddress(item.ShopAddress, ref shop))
                {
                    shop = new Shop
                    {
                        Name = Enum.GetName(typeof(ShopIdentifier), item.Shop),
                        Address = item.ShopAddress
                    };
                }
                var rCheck = new Repository.Entities.Check
                {
                    Date = DateTime.Now,
                    IsValid = true,
                    Shop = shop
                };
                var rProducts = item.Products.Select(product => new ProductListing
                {
                    Name = product.ProductEntry,
                    Price = product.Cost,
                    Check = rCheck
                });
                _listingRepo.SaveMany(rProducts);
            }
            catch (Exception e)
            {
                result = false;
                _log.Error(e);
            }
            return result;
        }
    }

}