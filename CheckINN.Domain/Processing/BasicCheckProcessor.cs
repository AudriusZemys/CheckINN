using System;
using System.Linq;
using CheckINN.Domain.Cache;
using CheckINN.Repository.Contexts;
using CheckINN.Repository.Entities;
using CheckINN.Repository.Repositories;
using Check = CheckINN.Domain.Entities.Check;

namespace CheckINN.Domain.Processing
{
    public class BasicCheckProcessor : ICheckProcessor
    {
        private readonly IRepository<Repository.Entities.Check> _checkRepo;
        private readonly IRepository<ProductListing> _listingRepo;

        public BasicCheckProcessor(IRepository<Repository.Entities.Check> checkRepo,
            IRepository<ProductListing> listingRepo)
        {
            _checkRepo = checkRepo;
            _listingRepo = listingRepo;
        }

        public bool TryProcess(Check item)
        {
            bool result = true;
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
                _checkRepo.Save(rCheck);
                _listingRepo.SaveMany(rProducts);
            }
            catch (Exception e)
            {
                result = false;
                Console.Write(e.Message);
            }
            return result;
        }
    }

}