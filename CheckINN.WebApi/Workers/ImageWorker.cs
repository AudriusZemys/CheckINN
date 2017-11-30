using System;
using System.Drawing;
using System.Threading;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Entities;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using CheckINN.Domain.Image;
using log4net;
using Unity;
using static System.Threading.Thread;

namespace CheckINN.WebApi.Workers
{
    /// <summary>
    /// Polls image queue for images and processes them
    /// </summary>
    public class ImageWorker : IDisposable
    {
        private readonly CancellationToken _cancellationToken;
        private readonly Thread _workerThread;
        private readonly IBitmapQueueCache _queue;
        private readonly IShopParser _parser;
        private readonly ICheckProcessor _processor;
        private readonly IUnityContainer _container;
        private readonly ILog _log;
        private readonly ITransform _transform;

        public delegate void ImageProcessedHandler(object sender, ImageProcessedEventArgs args);

        public virtual event ImageProcessedHandler ImageProcessed;

        public ImageWorker() {}

        public ImageWorker(CancellationToken cancellationToken, 
            IBitmapQueueCache queue, 
            IShopParser parser, 
            ICheckProcessor processor, 
            IUnityContainer container,
            ITransform transform,
            ILog log)
        {
            _cancellationToken = cancellationToken;
            _queue = queue;
            _parser = parser;
            _processor = processor;
            _container = container;
            _log = log;
            _transform = transform;
            _workerThread = new Thread(ProcessAllImages);
            _workerThread.Start();
        }

        private void ProcessAllImages()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                if (_queue.IsEmpty())
                {
                    Sleep(5000);
                    continue;
                };

                Bitmap image;
                try
                {
                    image = _queue.Dequeue();
                }
                catch (Exception)
                {
                    continue;
                }

                _transform.ToGreyscale(image);
                _transform.Sharpen(image);
                _transform.Brighten(image);
                var textRecognition = _container.Resolve<ITextRecognition>();
                textRecognition.Process(image);
                var ocrText = textRecognition.GetText();
                var products = _parser.ParseProductList(ocrText);
                var check = new Check(
                    checkBody: new CheckBody(products),
                    checkFooter: new CheckFooter("321654"),
                    checkHeader: new CheckHeader(ShopIdentifier.Maxima));

                textRecognition.Dispose();
                if (!_processor.TryProcess(check))
                {
                    _log.Error("Failed to process image");
                }

                if (ImageProcessed == null)
                {
                    _log.Info("Event squashed, no subscribers");
                }
                else
                {
                    ImageProcessed(this, new ImageProcessedEventArgs(ocrText));
                }
            }
        }

        public void Dispose()
        {
            _workerThread.Join();
        }

        protected virtual void OnImageProcessed(ImageProcessedEventArgs args)
        {
            ImageProcessed?.Invoke(this, args);
        }
    }
}
