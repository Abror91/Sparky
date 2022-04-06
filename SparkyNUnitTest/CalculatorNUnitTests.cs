using NUnit.Framework;
using Sparky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkyNUnitTest
{
    [TestFixture]
    public class CalculatorNUnitTests
    {
        private Calculator calculator;

        [SetUp]
        public void InitialSetup()
        {
            calculator = new Calculator();
        }
        [Test]
        public void AddNumbers_AddTwoNumbers_GetCorrectResult()
        {
            //Arrange

            //Act
            var result = calculator.AddNumbers(10, 20);

            //Assert
            Assert.AreEqual(30, result);
        }

        [Test]
        [TestCase(10, ExpectedResult = false)]
        [TestCase(5, ExpectedResult = true)]
        public bool IsOddNumber_ProvideEvenThenOddNumber_ReturnFalseThenTrue(int a)
        {
            return calculator.IsOddNumber(a);
        }


        [Test]
        [TestCase(5.4, 10.5)] // 15.9
        [TestCase(5.43, 10.53)] // 15.96
        [TestCase(5.49, 10.59)] // 16.08
        public void AddDoubleNumbers_InputDoubleNumbers_GetCorrectResultWithinRange(double a, double b)
        {
            var result = calculator.AddDoubleNumbers(a, b);

            Assert.AreEqual(15.9, result, 1); //Range between 14.9 and 16.9 are considered correct
        }

        [Test]
        public void GetOddRangeNumbers_ProvideMinMaxNumbers_GetOddNumbersRange()
        {
            //Arrange
            var expectedResult = new List<int> { 5, 7, 9 };

            //Act
            var result = calculator.GetOddNumbersRange(5, 10);

            //Assert
            Assert.That(result, Is.EquivalentTo(expectedResult));
            Assert.That(result, Does.Contain(7));
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result, Has.No.Member(6));
            Assert.That(result, Is.Ordered.Ascending);
            Assert.That(result, Is.Unique);
        }
    }
}
