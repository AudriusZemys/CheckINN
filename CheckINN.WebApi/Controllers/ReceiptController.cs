using System.Drawing;
using System.Web.Http;
using CheckINN.Domain.Entities;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using CheckINN.WebApi.Entities;

namespace CheckINN.WebApi.Controllers
{
    /// <summary>
    /// Endpoint for receipt images
    /// </summary>
    public class ReceiptController : ApiController
    {
        private readonly ITextRecognition _textRecognition;
        private readonly ICheckProcessor _processor;
        private readonly IShopParser _parser;

        private string _orcText;

        public ReceiptController(ITextRecognition textRecognition, ICheckProcessor processor, IShopParser parser)
        {
            _textRecognition = textRecognition;
            _processor = processor;
            _parser = parser;
        }

        /// <summary>
        /// Post image for processing
        /// </summary>
        /// <param name="image">Bitmap image of the receipt</param>
        /// <returns>Request outcome status</returns>
        [HttpPost] public Status PostReceipt([FromBody] Bitmap image)
        {
            _textRecognition.Process(image);  /*HERKUS DOES STUFF*/
            _orcText = _textRecognition.GetText(); /*Herkus smash*/
            var products = _parser.ParseProductList(_orcText);
            var check = new Check(
                checkBody: new CheckBody(products),
                checkFooter: new CheckFooter("321654"),
                checkHeader: new CheckHeader(ShopIdentifier.Maxima));

            if (!_processor.TryProcess(check))
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

        protected override void Dispose(bool disposing)
        {
            _textRecognition.Dispose();
            base.Dispose(disposing);
        }
    }
}
