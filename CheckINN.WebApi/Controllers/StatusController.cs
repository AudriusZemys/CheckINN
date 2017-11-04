using System.Web.Http;
using CheckINN.WebApi.Entities;

namespace CheckINN.WebApi.Controllers
{
    class StatusController : ApiController
    {
        public Status GetStatus()
        {
            return new Status(0, "OK");
        }
    }
}
