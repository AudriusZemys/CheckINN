using System.Web.Http;
using CheckINN.WebApi.Entities;

namespace CheckINN.WebApi.Controllers
{
    public class StatusController : ApiController
    {
        [HttpGet] public Status GetStatus()
        {
            return new Status(200, "OK");
        }
    }
}
