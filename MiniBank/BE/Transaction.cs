using MiniBank.InterfaceTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBank.BE
{
    public class Transaction : ITransaction
    {
        public int TransactionID { get; }

        public DateTime TransactionTime { get; }

        public string Message { get; }

        public double Amount { get; }

        public Transaction(int transID, string message, double amount)
        {
            TransactionID = transID;
            TransactionTime = DateTime.Now;
            Message = message;
            Amount = amount;
        }
    }
}
