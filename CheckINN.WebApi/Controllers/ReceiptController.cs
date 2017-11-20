using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using CheckINN.WebApi.Entities;

namespace CheckINN.WebApi.Controllers
{
    public class ReceiptController : ApiController
    {
        [HttpPost] public Status PostReceipt([FromBody] Bitmap image)
        {
            image.Save("C:\\receipt.bmp");
            return new Status(200, "OK");
        }
    }
}
