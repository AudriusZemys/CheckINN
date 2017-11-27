using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CheckINN.Domain.Cache;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Ploeh.AutoFixture;
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
            AreSame(expect, result);
        }
    }
}
