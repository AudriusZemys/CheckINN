using System;
using System.Linq;
using CheckINN.Repository.Entities;
using CheckINN.Repository.Repositories;
using log4net;
using Check = CheckINN.Domain.Entities.Check;

namespace CheckINN.Domain.Processing
{
    public class BasicCheckProcessor : ICheckProcessor
    {
        private readonly IRepository<ProductListing> _listingRepo;
        private readonly ILog _log;

        public BasicCheckProcessor(IRepository<ProductListing> listingRepo, ILog log)
        {
            _listingRepo = listingRepo;
            _log = log;
        }

        public bool TryProcess(Check item)
        {
            var result = true;
            try
            {
                var rCheck = new Repository.Entities.Check
                {
                    Date = DateTime.Now
                };
                var rProducts = item.CheckBody.Products.Select(product => new ProductListing
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