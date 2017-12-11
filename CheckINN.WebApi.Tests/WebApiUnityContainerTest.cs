using System;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.WebApi.Controllers;
using CheckINN.WebApi.Workers;
using log4net;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace CheckINN.WebApi.Tests
{
    [TestFixture]
    public class WebApiUnityContainerTest
    {
        /// <summary>
        /// This is just to validate build-time if 
        /// all dependencies are properly resolved.
        /// Saves some headaches
        /// </summary>
        [TestCase(typeof(IBitmapQueueCache))]
        [TestCase(typeof(ICheckProcessor))]
        [TestCase(typeof(IShopParser))]
        [TestCase(typeof(ILog))]
        [TestCase(typeof(ImageWorker))]
        [TestCase(typeof(ProductsController))]
        [TestCase(typeof(UserContoller))]
        [TestCase(typeof(ReceiptController))]
        public void UnityContainer_ResolvesAllClasses(Type type)
        {
            // arrange
            var host = new ApiHost();

            // act
            var result = host.Container.Resolve(type, null);

            // assert
            NotNull(result);
        }
    }
}
