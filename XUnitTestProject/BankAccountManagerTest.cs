using MiniBank.BE;
using MiniBank.BLL;
using MiniBank.InterfaceTypes;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestProject
{
    public class BankAccountManagerTest
    {
        // Mocking out the repository
        private Mock<IRepository<int, IBankAccount>> mockRepo = new Mock<IRepository<int, IBankAccount>>();
        
        // Data structure for the mock object
        // All the methods of the Mock object operates on this instance.
        private Dictionary<int, IBankAccount> accounts = new Dictionary<int, IBankAccount>();

        public BankAccountManagerTest()
        {
            mockRepo.Setup(x => x.Count).Returns(() => accounts.Count);
            mockRepo.Setup(x => x.Add(It.IsAny<IBankAccount>())).Callback<IBankAccount>(acc => { accounts.Add(acc.AccountNumber, acc); });
            mockRepo.Setup(x => x.Remove(It.IsAny<IBankAccount>())).Callback<IBankAccount>(acc => { accounts.Remove(acc.AccountNumber); });
            mockRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((accNum) => accounts[accNum]);
            mockRepo.Setup(x => x.GetAll()).Returns(() => new List<IBankAccount>(accounts.Values));
        }


        [Fact]
        // Test Creation of BankAccountManager with an empty repository
        public void CreateValidBankAccountManager()
        {
            IRepository<int, IBankAccount> repo = mockRepo.Object;
            BankAccountManager bam = new BankAccountManager(repo);
            Assert.Equal(0, repo.Count);
        }

        [Fact]
        // Test that repo is not null
        public void CreateInvalidBankAccountManagerExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                BankAccountManager bam = new BankAccountManager(null);
            });
        }

        [Fact]
        // Test adding a non-existing bank account to the repo
        public void AddValidBankAccount()
        {
            IRepository<int, IBankAccount> repo = mockRepo.Object;

            int accNumber = 1;
            IBankAccount acc = new BankAccount(accNumber);
            BankAccountManager bam = new BankAccountManager(repo);

            bam.AddBankAccount(acc);

            List<IBankAccount> allAccounts = repo.GetAll();
            Assert.True(allAccounts.Count == 1);
            IBankAccount fetchedAccount = allAccounts[0];
            Assert.True(acc == fetchedAccount);
        }

        [Fact]
        // Test adding a null value to the repository.
        public void AddInvalidBankAccountExpectArgumentException()
        {
            IRepository<int, IBankAccount> repo = mockRepo.Object;

            IBankAccount acc = null;
            BankAccountManager bam = new BankAccountManager(repo);

            Assert.Throws<ArgumentException>(() =>
            {
                bam.AddBankAccount(acc);
            });

            Assert.Equal(0, repo.Count);
        }

        [Fact]
        // test adding an already existing bank account to the repository.
        // The account is not added and an argumentException is thrown.
        public void AddExistingBankAccountExpectArgumentException()
        {
            IRepository<int, IBankAccount> repo = mockRepo.Object;

            IBankAccount acc = new BankAccount(1);
            BankAccountManager bam = new BankAccountManager(repo);
            bam.AddBankAccount(acc);
            Assert.Equal(1, repo.Count);

            Assert.Throws<ArgumentException>(() =>
            {
                bam.AddBankAccount(acc);
            });

            Assert.Equal(1, repo.Count);
            IBankAccount acc2 = repo.GetById(acc.AccountNumber);
            Assert.True(acc2 == acc);
        }

        [Fact]
        // Test removing an existing bank account.
        public void RemoveExistingBankAccount()
        {
            IRepository<int, IBankAccount> repo = mockRepo.Object;

            IBankAccount acc1 = new BankAccount(1);
            IBankAccount acc2 = new BankAccount(2);
            BankAccountManager bam = new BankAccountManager(repo);
            bam.AddBankAccount(acc1);
            bam.AddBankAccount(acc2);

            Assert.Equal(2, repo.Count);

            bam.RemoveBankAccount(acc1);

            Assert.Equal(1, repo.Count);
            IBankAccount result = repo.GetById(acc2.AccountNumber);
            Assert.True(result == acc2);
        }

        [Fact]
        // Test removing a non-existing bank account.
        // An argumentException should be thrown.
        public void RemoveNonExistingBankAccountExpectArgumentException()
        {
            IRepository<int, IBankAccount> repo = mockRepo.Object;

            IBankAccount acc1 = new BankAccount(1);
            IBankAccount acc2 = new BankAccount(2);

            BankAccountManager bam = new BankAccountManager(repo);
            bam.AddBankAccount(acc1);

           Assert.Equal(1, repo.Count);

            Assert.Throws<ArgumentException>(() =>
            {
                bam.RemoveBankAccount(acc2);
            });

            Assert.Equal(1, repo.Count);
            Assert.NotNull(repo.GetById(acc1.AccountNumber));
        }

        [Fact]
        // Test get an existing bank account by id.
        public void GetBankAccountByIdExistingBankAccount()
        {
            IRepository<int, IBankAccount> repo = mockRepo.Object;

            IBankAccount acc1 = new BankAccount(1);
            IBankAccount acc2 = new BankAccount(2);

            BankAccountManager bam = new BankAccountManager(repo);
            bam.AddBankAccount(acc1);
            bam.AddBankAccount(acc2);

            IBankAccount result = bam.GetBankAccountById(acc1.AccountNumber);

            Assert.True(acc1 == result);

        }

        [Fact]
        // Test get a non-existing bank account by id.
        // A null value should be returned.
        public void GetBankAccountByIdNonExistingBankAccountExpectNull()
        {
            IRepository<int, IBankAccount> repo = mockRepo.Object;

            IBankAccount acc1 = new BankAccount(1);
            IBankAccount acc2 = new BankAccount(2);

            BankAccountManager bam = new BankAccountManager(repo);
            bam.AddBankAccount(acc1);

            IBankAccount result = bam.GetBankAccountById(acc2.AccountNumber);

            Assert.Null(result);
        }
    }
}
