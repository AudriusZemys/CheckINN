using System.Threading;
using CheckINN.WebApi.Controllers;
using CheckINN.WebApi.Workers;
using Moq;
using NUnit.Framework;
using static System.Threading.Thread;
using static NUnit.Framework.Assert;

namespace CheckINN.WebApi.Tests.Controllers
{
    [TestFixture]
    public class NotificationControllerTest
    {
        [Test]
        public void PushNotification_WaitsForEventBeforeReplying()
        {
            // arrange
            var args = new ImageProcessedEventArgs("Some text");
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
            StringAssert.AreEqualIgnoringCase("Some text", text);

        }
    }
}
