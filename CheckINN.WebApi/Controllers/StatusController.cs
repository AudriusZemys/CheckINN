using System.Web.Http;
using CheckINN.WebApi.Entities;

namespace CheckINN.WebApi.Controllers
{
    /// <summary>
    /// Overall API status reporting class
    /// </summary>
    public class StatusController : ApiController
    {
        /// <summary>
        /// Debug if API is up or not
        /// </summary>
        /// <returns>Always positive API status</returns>
        [HttpGet] public Status GetStatus()
        {
            return new Status(true, "OK");
        }
    }
}
