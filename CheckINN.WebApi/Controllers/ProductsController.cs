using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CheckINN.Domain.Entities;
using CheckINN.Repository.Repositories;
using log4net;

namespace CheckINN.WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly ProductListingRepository _productRepository;
        private readonly CheckRepository _checkRepository;
        private readonly ILog _log;

        public ProductsController(ProductListingRepository productRepository, CheckRepository checkRepository, ILog log)
        {
            _productRepository = productRepository;
            _checkRepository = checkRepository;
            _log = log;
        }

        /// <summary>
        /// Will not test, because this will need to be tested with authorization later
        /// useless to do it now
        /// </summary>
        /// <returns>All products in the database</returns>
        [HttpGet] public IEnumerable<Product> GetAll()
        {
            var allCheckIds = _checkRepository.GetAll().Select(check => check.CheckId);
            var products = allCheckIds.SelectMany(id => _productRepository.GetByCheckId(id));
            return products.Select(listing => new Product(listing.Name, listing.Price));
        }

        [HttpGet] public IEnumerable<Product> GetByCheckId(int checkId)
        {
            return _productRepository.GetByCheckId(checkId).Select(listing => new Product(listing.Name, listing.Price));
        }
    }
}
