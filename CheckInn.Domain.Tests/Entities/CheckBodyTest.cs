using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckINN.Domain.Entities;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace CheckInn.Domain.Tests.Entities
{
    [TestFixture]
    class CheckBodyTest
    {
        [Test]
        public void CheckForSingleEnumerableIteration()
        {
            var fixture = new Fixture().Customize(new MultipleCustomization());
            var check = fixture.Create<CheckBody>();

            var failed = false;
            try
            {
                var first = check[1];
                var products = check.Products;
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.False(failed);
        }
    }
}
