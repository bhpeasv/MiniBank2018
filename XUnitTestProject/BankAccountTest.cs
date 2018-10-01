using MiniBank.BE;
using MiniBank.InterfaceTypes;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject
{
    public class BankAccountTest
    {
        [Fact]
        public void CreateValidBankAccountWithZeroBalance()
        {
            int accNumber = 1;

            IBankAccount acc = new BankAccount(accNumber);

            Assert.Equal(accNumber, acc.AccountNumber);
            Assert.Equal(0.00, acc.Balance);
            Assert.Equal(BankAccount.DEFAULT_INTEREST_RATE, acc.InterestRate);

            Assert.Single<ITransaction>(acc.TransactionList);
            ITransaction expectedTransaction = new Transaction(1, "Account Created with balance", 0.00);
            Assert.True(AreEqualTransactions(expectedTransaction, acc.TransactionList[0]));
        }

        [Fact]
        public void CreateValidBankAccountWithInitialBalance()
        {
            int accNumber = 1;
            double initialBalance = 1234.56;

            IBankAccount acc = new BankAccount(accNumber, initialBalance);

            Assert.Equal(accNumber, acc.AccountNumber);
            Assert.Equal(initialBalance, acc.Balance);
            Assert.Equal(BankAccount.DEFAULT_INTEREST_RATE, acc.InterestRate);
            Assert.Single<ITransaction>(acc.TransactionList);
            ITransaction expectedTransaction = new Transaction(1, "Account Created with balance", initialBalance);
            Assert.True(AreEqualTransactions(expectedTransaction, acc.TransactionList[0]));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateInvalidBankAccountWithZeroBalanceExpectArgumentException(int accNumber)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                IBankAccount acc = new BankAccount(accNumber);
            });
        }

        [Theory]
        [InlineData(0, 1234.56)]
        [InlineData(-1, 1234.56)]
        [InlineData(1, -0.01)]
        public void CreateInvalidBankAccountWithInitialBalanceExpectArgumentException(int accNumber, double initialBalance)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                IBankAccount acc = new BankAccount(accNumber, initialBalance);
            });
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(1234.56)]
        public void DepositValidAmount(double amount)
        {
            double initialBalance = 123.45;
            IBankAccount acc = new BankAccount(1, initialBalance);
            Assert.Single(acc.TransactionList);

            acc.Deposit(amount);

            Assert.Equal(initialBalance + amount, acc.Balance);
            Assert.True(acc.TransactionList.Count == 2);
            ITransaction t = new Transaction(2, "Deposit", amount);
            Assert.True(AreEqualTransactions(t, acc.TransactionList[1]));         
        }

        [Theory]
        [InlineData(0.00)]
        [InlineData(-1234.56)]
        public void DepositInvalidAmountExpectArgumentException(double amount)
        {
            double initialBalance = 123.45;
            IBankAccount acc = new BankAccount(1, initialBalance);
            Assert.Single(acc.TransactionList);
            try
            {
                acc.Deposit(amount);
                Assert.True(false, "Deposit of non-positive amount");
            }
            catch (ArgumentException)
            {
                Assert.Equal(initialBalance, acc.Balance);
                Assert.Single(acc.TransactionList);
            }
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(123.45)]
        public void WithdrawValidAmount(double amount)
        {
            double initialBalance = 123.45;
            IBankAccount acc = new BankAccount(1, initialBalance);
            Assert.Single(acc.TransactionList);

            acc.Withdraw(amount);

            Assert.Equal(initialBalance - amount, acc.Balance);
            Assert.True(acc.TransactionList.Count == 2);
            ITransaction t = new Transaction(2, "Withdraw", -amount);
            Assert.True(AreEqualTransactions(t, acc.TransactionList[1]));
        }

        [Theory]
        [InlineData(0.00, 123.45)]
        [InlineData(-0.01, 123.45)]
        [InlineData(123.46, 123.45)]
        public void WithdrawInvalidAmountExpectArgumentException(double amount, double initialBalance)
        {
            IBankAccount acc = new BankAccount(1, initialBalance);

            try
            {
                acc.Withdraw(amount);
                Assert.True(false, "Withdraw of invalid amount");
            }
            catch (ArgumentException)
            {
                Assert.Equal(initialBalance, acc.Balance);
            }
        }

        [Theory]
        [InlineData(0.00)]
        [InlineData(0.05)]
        [InlineData(0.10)]
        public void SetValidInterestRate(double newInterestRate)
        {
            IBankAccount acc = new BankAccount(1);

            acc.InterestRate = newInterestRate;

            Assert.Equal(newInterestRate, acc.InterestRate);
        }

        [Theory]
        [InlineData(-0.001)]
        [InlineData(10.001)]
        public void SetInvalidInterestRateExpectArgumentException(double newInterestRate)
        {
            IBankAccount acc = new BankAccount(1);
            double oldInterestRate = acc.InterestRate;

            try
            {
                acc.InterestRate = newInterestRate;
                Assert.True(false, "Set invalid interest rate");
            }
            catch (ArgumentException)
            {
                Assert.Equal(oldInterestRate, acc.InterestRate);
            }
        }

        [Fact]
        public void AddInterest()
        {
            double initialBalance = 1234.56;
            IBankAccount acc = new BankAccount(1, initialBalance);
            acc.AddInterest();

            Assert.Equal(initialBalance * (1 + acc.InterestRate), acc.Balance);
        }

        private bool AreEqualTransactions(ITransaction t1, ITransaction t2)
        {
            return t1.TransactionID == t2.TransactionID
                && t1.Message == t2.Message
                && t1.Amount == t2.Amount;
        }
    }
}
