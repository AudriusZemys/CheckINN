using System.Threading;
using CheckINN.Domain.Entities;
using CheckINN.WebApi.Controllers;
using CheckINN.WebApi.Workers;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using static System.Threading.Thread;
using static NUnit.Framework.Assert;

namespace CheckINN.WebApi.Tests.Controllers
{
    [TestFixture]
    public class NotificationControllerTest
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void PushNotification_WaitsForEventBeforeReplying()
        {
            // arrange
            var args = _fixture.Create<ImageProcessedEventArgs>();
            var mockImageWorker = new Mock<ImageWorker>();
            var controller = new NotificationController(mockImageWorker.Object);
            object result = null;
            var request = new Thread(() =>
            {
                result = controller.PushNotification();
            });

            // act
            request.Start();
            Sleep(1000);
            mockImageWorker.Raise(worker => worker.ImageProcessed += null, args);
            Sleep(1000);

            // assert
            AreEqual(ThreadState.Stopped, request.ThreadState);

            NotNull(result);
            var type = result.GetType();
            var property = type.GetProperty("ocrText");
            var text = (string)property?.GetValue(result, null);
            NotNull(text);
            StringAssert.AreEqualIgnoringCase(args.OcrText, text);

        }
    }
}
