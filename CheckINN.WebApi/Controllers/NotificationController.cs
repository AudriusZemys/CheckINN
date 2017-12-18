using System.Collections.Generic;
using System.Web.Http;
using CheckINN.Domain.Entities;
using CheckINN.WebApi.Workers;
using static System.Threading.Thread;

namespace CheckINN.WebApi.Controllers
{
    /// <summary>
    /// Event based push notifications
    /// </summary>
    /// TODO: Use an EventBus as an improvment
    public class NotificationController : ApiController
    {
        private readonly ImageWorker _imageWorker;
        private volatile IEnumerable<Product> _products;
        private volatile string _ocrText;

        public NotificationController(ImageWorker imageWorker)
        {
            _ocrText = null;
            _imageWorker = imageWorker;
            _imageWorker.ImageProcessed += OnImageProcessed;
        }

        /// <summary>
        /// When any image is processed an event is raised and caught here
        /// </summary>
        /// <param name="sender">Who sent the event</param>
        /// <param name="args">Event args</param>
        private void OnImageProcessed(object sender, ImageProcessedEventArgs args)
        {
            _ocrText = args.OcrText;
            _products = args.Products;
        }

        /// <summary>
        /// Get request that does not returned until timeout or until an event is caught
        /// </summary>
        /// <returns>If proceesing is successful and resulting text</returns>
        [HttpGet] public object PushNotification()
        {
            while (_ocrText == null)
            {
                Sleep(100);
            }

            return new
            {
                success = true,
                ocrText = _ocrText,
                products = _products
            };
        }

        /// <summary>
        /// Unsubscribe from event
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageWorker.ImageProcessed -= OnImageProcessed;
            }
            base.Dispose(disposing);
        }
    }
}
