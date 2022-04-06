using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    public class BankAccount
    {
        private int balance;
        private ILogBook _logBook;
        public BankAccount(ILogBook logBook)
        {
            balance = 0;
            _logBook = logBook;
        }

        public bool Deposit(int amount)
        {
            if(amount > 100)
            {
                _logBook.LogMessage("Testing");
                _logBook.LogSeverity = 101;
            }
            _logBook.LogMessage("Deposit was invoked");
            var temp = _logBook.LogSeverity;
            balance += amount;
            return true;
        }

        public bool Withdraw(int amount)
        {
            if(amount <= balance)
            {
                _logBook.LogToDB("Withdrawal amount: " + amount.ToString());
                balance -= amount;
                return _logBook.LogBalanceAfterWithdrawal(balance);
            }
            return _logBook.LogBalanceAfterWithdrawal(balance - amount);
        }

        public int GetBalance()
        {
            return balance;
        }
    }
}
