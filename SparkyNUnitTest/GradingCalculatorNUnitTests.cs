using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    [TestFixture]
    public class GradingCalculatorNUnitTests
    {
        private GradingCalculator gradingCalculator;

        [SetUp]
        public void InitialSetup()
        {
            gradingCalculator = new GradingCalculator();
        }

        [Test]
        [TestCase(95, 95, ExpectedResult = "A")]
        [TestCase(85, 90, ExpectedResult = "B")]
        [TestCase(65, 90, ExpectedResult = "C")]
        [TestCase(95, 65, ExpectedResult = "B")]
        [TestCase(95, 55, ExpectedResult = "F")]
        public string GetGrade_InputScoreAndAttandance_ReturnsGrade(int score, int attandance)
        {
            gradingCalculator.Score = score;
            gradingCalculator.AttandancePercentage = attandance;
            return gradingCalculator.GetGrade();
        }
    }
}
