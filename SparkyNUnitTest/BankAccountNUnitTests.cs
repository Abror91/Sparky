using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    [TestFixture]
    public class BankAccountNUnitTests
    {
        [Test]
        public void Deposit_Add100_ReturnsTrue()
        {
            var loggingMock = new Mock<ILogBook>();
            BankAccount bankAccount = new BankAccount(loggingMock.Object);

            var result = bankAccount.Deposit(100);

            Assert.IsTrue(result);
            Assert.AreEqual(100, bankAccount.GetBalance());
        }

        [Test]
        public void Widthraw_Deposit200Withdraw100_ReturnsTrue()
        {
            var loggingMock = new Mock<ILogBook>();
            loggingMock.Setup(s => s.LogToDB(It.IsAny<string>())).Returns(true); //Not really necessary setup
            loggingMock.Setup(s => s.LogBalanceAfterWithdrawal(It.Is<int>(s => s >= 0))).Returns(true);

            BankAccount bankAccount = new BankAccount(loggingMock.Object);

            bankAccount.Deposit(200);
            var result = bankAccount.Withdraw(100);

            Assert.IsTrue(result);
        }

        [Test]
        public void Widthraw_Deposit100Widthraw200_ReturnsFalse()
        {
            var loggingMock = new Mock<ILogBook>();
            loggingMock.Setup(s => s.LogBalanceAfterWithdrawal(It.Is<int>(s => s < 0))).Returns(false);

            BankAccount bankAccount = new BankAccount(loggingMock.Object);

            bankAccount.Deposit(100);
            var result = bankAccount.Withdraw(200);

            Assert.IsFalse(result);
        }

        [Test]
        public void DummyLog_SetupMessageWithReturnString_ReturnsString()
        {
            var desiredOutput = "hello";
            var loggingMock = new Mock<ILogBook>();

            loggingMock.Setup(s => s.MessageWithReturnStr(It.IsAny<string>())).Returns((string s) => s.ToLower());

            Assert.That(loggingMock.Object.MessageWithReturnStr("HELLO"), Is.EqualTo(desiredOutput));
        }

        [Test]
        public void DummyLog_SetupLogWithOutputResult_ReturnsString()
        {
            var loggingMock = new Mock<ILogBook>();
            string outputResult = "hello";

            loggingMock.Setup(s => s.LogWithOutputResult(It.IsAny<string>(), out outputResult)).Returns(true);

            string result = "";

            Assert.IsTrue(loggingMock.Object.LogWithOutputResult("testing", out result));
            Assert.That(result, Is.EqualTo(outputResult));
        }

        [Test]
        public void DummyLog_SetupLogWithRefObject_ReturnsTrue()
        {
            var logginMock = new Mock<ILogBook>();
            var customer = new Customer();
            var customerNotUser = new Customer();

            logginMock.Setup(s => s.LogWithRefObject(ref customer)).Returns(true);

            Assert.IsTrue(logginMock.Object.LogWithRefObject(ref customer));
            Assert.IsFalse(logginMock.Object.LogWithRefObject(ref customerNotUser));
        }

        [Test]
        public void LogDummy_SetAndGetLogSeverityAndLogType_MockTest()
        {
            var loggingMock = new Mock<ILogBook>();
            loggingMock.Setup(s => s.LogSeverity).Returns(10);
            loggingMock.Setup(s => s.LogType).Returns("Warning");

            //loggingMock.SetupAllProperties();
            //loggingMock.Object.LogSeverity = 100;
            //loggingMock.Object.LogType = "Warning";

            Assert.That(loggingMock.Object.LogSeverity, Is.EqualTo(10));
            Assert.That(loggingMock.Object.LogType, Is.EqualTo("Warning"));

            //callbacks
            string logTemp = "Hello, ";
            loggingMock.Setup(s => s.LogToDB(It.IsAny<string>())).Returns(true).Callback((string str) => logTemp += str);

            loggingMock.Object.LogToDB("Ben");

            Assert.That(logTemp, Is.EqualTo("Hello, Ben"));

            //callbacks
            int counter = 5;
            loggingMock.Setup(s => s.LogToDB(It.IsAny<string>())).Returns(true).Callback(() => counter++);

            loggingMock.Object.LogToDB("Ben");
            loggingMock.Object.LogToDB("Ben");

            Assert.That(counter, Is.EqualTo(7));
        }

        [Test]
        public void Deposit_Deposit200_CallsLogMessageTwiceLogSeverityGetterAndSettersOnce()
        {
            var loggingMock = new Mock<ILogBook>();
            var bankAccount = new BankAccount(loggingMock.Object);

            bankAccount.Deposit(200);

            Assert.That(bankAccount.GetBalance(), Is.EqualTo(200));

            //verification
            loggingMock.Verify(s => s.LogMessage(It.IsAny<string>()), Times.Exactly(2));
            loggingMock.Verify(s => s.LogMessage("Testing"), Times.AtLeastOnce);
            loggingMock.VerifySet(s => s.LogSeverity = 101, Times.Once);
            loggingMock.VerifyGet(s => s.LogSeverity, Times.Once);
        }

        [Test]
        public void Deposit_Deposit50_CallsLogMessageOnceLogSeverityGetterOnce()
        {
            var loggingMock = new Mock<ILogBook>();
            var bankAccount = new BankAccount(loggingMock.Object);

            bankAccount.Deposit(50);

            Assert.AreEqual(50, bankAccount.GetBalance());

            //verifications
            loggingMock.Verify(s => s.LogMessage(It.IsAny<string>()), Times.Once);
            loggingMock.Verify(s => s.LogMessage("Testing"), Times.Never);
            loggingMock.VerifyGet(s => s.LogSeverity, Times.Once);
        }
    }
}
