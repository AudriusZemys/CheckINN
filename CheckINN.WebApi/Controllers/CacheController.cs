using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Entities;
using Unity.Interception.Utilities;

namespace CheckINN.WebApi.Controllers
{
    public class CacheController : ApiController
    {
        private readonly ICheckCache _cache;

        public CacheController(ICheckCache cache)
        {
            _cache = cache;
        }

        [HttpGet] public IEnumerable<Product> GetAllProducts()
        {
            return _cache.Select(check => check.CheckBody).SelectMany(body => body.Products);
        }
    }
}
