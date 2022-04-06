using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    [TestFixture]
    public class ProductNUnitTests
    {
        [Test]
        public void GetPrice_InputPlatinumCustomer_ReturnsDiscountedPrice()
        {
            Product product = new Product() { Price = 50 };

            var result = product.GetPrice(new Customer { IsPlatinum = true });

            Assert.That(result, Is.EqualTo(40));
        }
    }
}
