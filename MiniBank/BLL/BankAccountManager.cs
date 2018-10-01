using MiniBank.InterfaceTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBank.BLL
{
    public class BankAccountManager
    {
        private IRepository<int, IBankAccount> accountsRepo;

        public int Count => accountsRepo.Count;

        public BankAccountManager(IRepository<int, IBankAccount> repository)
        {
            accountsRepo = repository ?? throw new ArgumentException("repository cannot be null");
        }

        public void AddBankAccount(IBankAccount acc)
        {
            if (acc == null)
                throw new ArgumentException("No Bank Account to add");
            try
            {
                IBankAccount result = accountsRepo.GetById(acc.AccountNumber);
                throw new ArgumentException("Bank account already exist");
            }
            catch (KeyNotFoundException)
            {
                accountsRepo.Add(acc);
            }
        }

        public void RemoveBankAccount(IBankAccount acc)
        {
            try
            {
                IBankAccount result = accountsRepo.GetById(acc.AccountNumber);
                accountsRepo.Remove(acc);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Bank Account does not exist");
            }
        }

        public IBankAccount GetBankAccountById(int accountNumber)
        {
            try
            {
                return accountsRepo.GetById(accountNumber);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
