using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBank.InterfaceTypes
{
    public interface ITransaction
    {
        int TransactionID { get; }
        DateTime TransactionTime { get; }
        String Message { get; }
        double Amount { get; }
    }
}
