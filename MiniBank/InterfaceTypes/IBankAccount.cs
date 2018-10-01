using System.Collections.Generic;

namespace MiniBank.InterfaceTypes
{
    public interface IBankAccount
    {
        int AccountNumber { get; }
        double Balance { get; }
        double InterestRate { get; set; }
        List<ITransaction> TransactionList { get; }

        void Deposit(double amount);
        void Withdraw(double amount);
        void AddInterest();
    }
}
