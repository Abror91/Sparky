using Bongo.Models.ModelValidations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Models.Tests
{
    [TestFixture]
    public class DateInFutureAttributeTests
    {
        [Test]
        [TestCase(100, ExpectedResult = true)]
        [TestCase(-100, ExpectedResult = false)]
        [TestCase(0, ExpectedResult = false)]
        public bool DateInFuture_InputExpectedDataRange_DateValidity(int seconds)
        {
            DateInFutureAttribute attribute = new DateInFutureAttribute(() => DateTime.Now);

            return attribute.IsValid(DateTime.Now.AddSeconds(seconds));
        }

        [Test]
        public void DataInFuture_CreateValidatorAttribute_CheckErrorMessageExists()
        {
            var attribute = new DateInFutureAttribute();
            Assert.AreEqual("Date must be in the future", attribute.ErrorMessage);
        }
    }
}
