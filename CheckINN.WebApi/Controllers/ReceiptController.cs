using System;
using System.Drawing;
using System.Web.Http;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Entities;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using CheckINN.WebApi.Entities;
using CheckINN.Domain.Image;
using log4net;

namespace CheckINN.WebApi.Controllers
{
    /// <summary>
    /// Endpoint for receipt images
    /// </summary>
    public class ReceiptController : ApiController
    {
        private readonly Lazy<ITextRecognition> _textRecognition;
        private readonly Lazy<ICheckProcessor> _processor;
        private readonly Lazy<IShopParser> _parser;
        private readonly Lazy<ITransform> _transformer;
        private readonly ILog _log;
        private readonly IBitmapQueueCache _queue;

        private string _orcText;

        public ReceiptController(Lazy<ITextRecognition> textRecognition, 
            Lazy<ICheckProcessor> processor, 
            Lazy<IShopParser> parser,
            Lazy<ITransform> transformer,
            ILog log, 
            IBitmapQueueCache queue)
        {
            _textRecognition = textRecognition;
            _processor = processor;
            _parser = parser;
            _transformer = transformer;
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

        /// <summary>
        /// Post image for processing
        /// </summary>
        /// <param name="image">Bitmap image of the receipt</param>
        /// <returns>Request outcome status</returns>
        [HttpPost] public Status PostReceipt([FromBody] Bitmap image)
        {
            /*Image transforming*/
            _transformer.Brighten(image);
            _transformer.ToGreyscale(image);
            _transformer.Sharpen(image);
            _textRecognition.Process(image);
            _orcText = _textRecognition.GetText();
            var products = _parser.ParseProductList(_orcText);
            var textRecognition = _textRecognition.Value;
            var parser = _parser.Value;
            var processor = _processor.Value;
            textRecognition.Process(image);
            _orcText = textRecognition.GetText();
            var products = parser.ParseProductList(_orcText);
            var check = new Check(
                checkBody: new CheckBody(products),
                checkFooter: new CheckFooter("321654"),
                checkHeader: new CheckHeader(ShopIdentifier.Maxima));

            if (!processor.TryProcess(check))
            {
                return new Status(false, "Cannot process check");
            }
            return new Status(true, "OK");
        }

        /// <summary>
        /// Same as PostReceipt, except it returns text that's in the receipt
        /// </summary>
        /// <param name="image">Bitmap image of the receipt</param>
        /// <returns>Request outcome status and text output from OCR processor</returns>
        [HttpPost] public object PostReceiptGetText([FromBody] Bitmap image)
        {
            var resultStatus = PostReceipt(image);
            return new
            {
                ocrText = _orcText,
                success = resultStatus.Success,
                message = resultStatus.Message
            };
        }
    }
}
