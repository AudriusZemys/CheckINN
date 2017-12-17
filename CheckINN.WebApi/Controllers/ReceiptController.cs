using System;
using System.Drawing;
using System.Web.Http;
using CheckINN.Domain.Cache;
using CheckINN.WebApi.Entities;
using log4net;

namespace CheckINN.WebApi.Controllers
{
    /// <summary>
    /// Endpoint for receipt images
    /// </summary>
    public class ReceiptController : ApiController
    {
        private readonly ILog _log;
        private readonly IBitmapQueueCache _queue;

        public ReceiptController(ILog log, 
            IBitmapQueueCache queue)
        {
            _log = log;
            _queue = queue;
        }

        /// <summary>
        /// Send off image for asynchronous processing
        /// </summary>
        /// <param name="image">Bitmap image of the receipt</param>
        /// <returns>Request outcome status</returns>
        [HttpPost] public Status DispatchReceipt([FromBody] Bitmap image)
        {
            try
            {
                _queue.Put(image);
            }
            catch (Exception exception)
            {
                _log.Error($"Failed to enqueue image - {exception}");
                return new Status(false, "Failed to enqueue image");
            }
            return new Status(true, "Enqueued image");
        }
    }
}
