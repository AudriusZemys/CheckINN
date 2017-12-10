using System;
using System.Drawing;
using System.Reflection;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Image;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using CheckINN.WebApi.Controllers;
using log4net;
using Moq;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace CheckINN.WebApi.Tests.Controllers
{
    /// <summary>
    /// Only test the dispatch endpoint.
    /// The others will not be used in time
    /// </summary>
    [TestFixture]
    public class ReceiptControllerTest
    {
        [Test]
        public void DispatchReceipt_PutsReceiptInQueue()
        {
            // arrange
            var queueCache = new BitmapQueueCache();
            var sample = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("CheckINN.WebApi.Tests.test_sample.bmp");
            NotNull(sample);
            var expect = new Bitmap(sample);

            var textRecognition = new Mock<ITextRecognition>();
            var processor = new Mock<ICheckProcessor>();
            var parser = new Mock<IShopParser>();
            var transform = new Mock<ITransform>();
            var controller = new ReceiptController(
                new Lazy<ITextRecognition>(() => textRecognition.Object),
                new Lazy<ICheckProcessor>(() => processor.Object), 
                new Lazy<IShopParser>(() => parser.Object), 
                new Lazy<ITransform>(() => transform.Object), 
                LogManager.GetLogger(GetType().FullName),
                queueCache);

            // act
            var status = controller.DispatchReceipt(expect);
            var result = queueCache.Dequeue();

            // assert
            // This is a bit wrong, because we don't know if it's ever reformed properly
            // Also might break with underneath changes
            AreSame(expect, result); 
            IsTrue(status.Success);
            StringAssert.AreEqualIgnoringCase("Enqueued image", status.Message);
        }
    }
}
