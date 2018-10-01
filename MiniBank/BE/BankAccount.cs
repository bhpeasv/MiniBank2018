using MiniBank.InterfaceTypes;
using System;
using System.Collections.Generic;

namespace MiniBank.BE
{
    public class BankAccount : IBankAccount
    {
        public const double DEFAULT_INTEREST_RATE = 0.01;

        private double interestRate;

        public int AccountNumber { get; private set; }
        public double Balance { get; private set; }
        public double InterestRate
        {
            get
            { return interestRate; }
            set
            {
                if (value < 0.00 || value > 0.10)
                    throw new ArgumentException("Invalid interest rate");
                interestRate = value;
            }
        }

        public List<ITransaction> TransactionList { get; }

        public BankAccount(int accNumber)
            : this(accNumber, 0.00)
        {
        }

        public BankAccount(int accNumber, double initialBalance)
        {
            if (accNumber < 1)
                throw new ArgumentException("Account number cannot be negative");
            if (initialBalance < 0)
                throw new ArgumentException("Initial Balance cannot be negative");

            AccountNumber = accNumber;
            Balance = initialBalance;
            InterestRate = DEFAULT_INTEREST_RATE;
            TransactionList = new List<ITransaction>();
            TransactionList.Add(new Transaction(1, "Account Created with balance", initialBalance));
        }

        public void Deposit(double amount)
        {
            if (amount <= 0.00)
                throw new ArgumentException("Amount to deposit must be positive");
            Balance += amount;
            TransactionList.Add(new Transaction(TransactionList.Count + 1, "Deposit", amount));
        }

        public void Withdraw(double amount)
        {
            if (amount <= 0.00)
                throw new ArgumentException("Amount to withdraw must be positive");
            if (amount > Balance)
                throw new ArgumentException("Amount to withdraw cannot exceed the Balance");

            Balance -= amount;
            TransactionList.Add(new Transaction(TransactionList.Count + 1, "Withdraw", -amount));
        }

        public void AddInterest()
        {
            Balance += (Balance * InterestRate);
        }
    }
}
