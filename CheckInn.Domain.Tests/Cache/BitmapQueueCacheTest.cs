using System.Drawing;
using System.Reflection;
using CheckINN.Domain.Cache;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace CheckInn.Domain.Tests.Cache
{
    [TestFixture]
    public class BitmapQueueCacheTest
    {
        [Test]
        public void PutAndDequeueOnTheSameObject()
        {
            // arrange
            var sample = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("CheckInn.Domain.Tests.test_sample.bmp");
            NotNull(sample);
            var expect = new Bitmap(sample);
            var cache = new BitmapQueueCache();

            // act
            cache.Put(expect);
            var result = cache.Dequeue();

            // assert
            // This is a bit wrong, because we don't know if it's ever reformed properly
            // Also might break with underneath changes
            AreSame(expect, result);
        }
    }
}
