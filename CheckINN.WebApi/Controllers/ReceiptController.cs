using System.Drawing;
using System.Web.Http;
using CheckINN.Domain.Entities;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using CheckINN.WebApi.Entities;

namespace CheckINN.WebApi.Controllers
{
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

        [HttpPost] public Status PostReceipt([FromBody] Bitmap image)
        {
            _textRecognition.Process(image);
            _orcText = _textRecognition.GetText();
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
