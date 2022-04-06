using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    [TestFixture]
    public class CustomerNUnitTests
    {
        private Customer customer;

        [SetUp]
        public void InitialSetup()
        {
            customer = new Customer();
        }
        [Test]
        public void CombineNames_InputFirstAndLastnames_GetFullname()
        {
            //Arrange

            //Act
            customer.GreetAndCombineNames("Ben", "Spark");

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(customer.GreetMessage, "Hello, Ben Spark");
                Assert.That(customer.GreetMessage, Is.EqualTo("Hello, Ben Spark"));
                Assert.That(customer.GreetMessage, Does.Contain("ben Spark").IgnoreCase);
                Assert.That(customer.GreetMessage, Does.StartWith("Hello,"));
                Assert.That(customer.GreetMessage, Does.EndWith("Spark"));
                Assert.That(customer.GreetMessage, Does.Match("Hello, [A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+"));
            });
        }

        [Test]
        public void GreetMessage_NotGreeted_ReturnsNull()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsNull(customer.GreetMessage);
        }

        [Test]
        public void Discount_defaultCoustomer_ShouldBeInRange10And25()
        {
            var result = customer.Discount;

            Assert.That(result, Is.InRange(10, 25));
        }

        [Test]
        public void GreetAndCombineNames_CallWithoutLastname_GreetMessageNotNull()
        {
            customer.GreetAndCombineNames("ben", "");

            Assert.IsNotNull(customer.GreetMessage);
            Assert.IsFalse(string.IsNullOrWhiteSpace(customer.GreetMessage));
        }

        [Test]
        public void GreetAndCombineNames_InputEmptyFirstname_ThrowsArgementException()
        {
            var exceptionDetails = Assert.Throws<ArgumentException>(() => customer.GreetAndCombineNames("", "spark"));
            Assert.AreEqual("Empty firstname", exceptionDetails.Message);

            Assert.That(() => customer.GreetAndCombineNames("", "spark"), 
                Throws.ArgumentException.With.Message.EqualTo("Empty firstname"));
        }

        [Test]
        public void GetCustomerDetails_SeOrderTotalToLessThan100_ReturnsBasicCustomer()
        {
            customer.OrderTotal = 10;
            var result = customer.GetCustomerDetails();
            Assert.That(result, Is.TypeOf<BasicCustomer>());
        }

        [Test]
        public void GetCustomerDetails_SetOrderDetailsToMoreThan100_ReturnsPlatinumCustomer()
        {
            customer.OrderTotal = 101;
            var result = customer.GetCustomerDetails();
            Assert.That(result, Is.TypeOf<PlatinumCustomer>());
        }
    }
}
