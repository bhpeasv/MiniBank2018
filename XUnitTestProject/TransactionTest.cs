using MiniBank.BE;
using MiniBank.InterfaceTypes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestProject
{
    public class TransactionTest
    {
        [Theory]
        [InlineData(1, "Transaction Text", 1234.56)]
        public void CreateValidTransaction(int transID, string message, double amount)
        {
            DateTime before = DateTime.Now;
            ITransaction t = new Transaction(transID, message, amount);
            DateTime after = DateTime.Now;

            Assert.True(before <= t.TransactionTime && t.TransactionTime <= after);
            Assert.Equal(transID, t.TransactionID);
            Assert.Equal(message, t.Message);
            Assert.Equal(amount, t.Amount);
        }
    }
}
