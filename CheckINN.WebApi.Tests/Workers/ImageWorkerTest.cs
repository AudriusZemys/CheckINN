using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Entities;
using CheckINN.Domain.Image;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using CheckINN.WebApi.Workers;
using log4net;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Unity;

namespace CheckINN.WebApi.Tests.Workers
{
    [TestFixture]
    public class ImageWorkerTest
    {
        private Fixture _fixture;
        private Queue<Bitmap> _queue;

        private class Dependencies
        {
            public Mock<ITextRecognition> Recognition { get; set; }
            public CancellationTokenSource CancellationTokenSource { get; set; }
            public Mock<IBitmapQueueCache> Queue { get; set; }
            public Mock<IShopParser> Parser { get; set; }
            public Mock<ICheckProcessor> Processor { get; set; }
            public IUnityContainer Container { get; set; }
            public Mock<ITransform> Transform { get; set; }
            public ILog Log { get; set; }
        }

        private Dependencies MockDependencies()
        {
            var textRecognition = new Mock<ITextRecognition>();
            textRecognition.Setup(recognition => recognition.GetText()).Returns("OCR Text");
            var parser = new Mock<IShopParser>();
            parser.Setup(shopParser => shopParser.ParseProductList(It.IsAny<string>()))
                .Returns(() => _fixture.CreateMany<Product>());
            var processor = new Mock<ICheckProcessor>();
            processor.Setup(checkProcessor => checkProcessor.TryProcess(It.IsAny<Check>())).Returns(true);
            var bitmapQueue = new Mock<IBitmapQueueCache>();
            bitmapQueue.Setup(cache => cache.Put(It.IsAny<Bitmap>()))
                .Callback<Bitmap>(bitmap => _queue.Enqueue(bitmap));
            var transform = new Mock<ITransform>();
            return new Dependencies
            {
                Parser = parser,
                Processor = processor,
                Queue = bitmapQueue,
                Recognition = textRecognition,
                Transform = transform,
                Container = new UnityContainer(),
                Log = LogManager.GetLogger("ImageWorkerTest"),
                CancellationTokenSource = new CancellationTokenSource()
            };
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _queue = new Queue<Bitmap>();
        }

        /// TODO: Unfinished. Realized that not only bitmaps need to be passed around. Useless to try test now.
        [Test]
        public void SendsNotificationWhenImageIsProcessed()
        {
            // arrange
            var deps = MockDependencies();
            var worker = new ImageWorker(
                deps.CancellationTokenSource.Token,
                deps.Queue.Object,
                deps.Parser.Object,
                deps.Processor.Object,
                deps.Container,
                deps.Transform.Object,
                deps.Log
                );
            // TODO: Will be continued
        }
    }
}
