using Dapper;
using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Repositories;
using System;
using System.Linq;
using Xunit;

namespace SmartVault.Tests.Unit
{
    public class AccountRepositoryTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture _dbFixture;
        IAccountRepository _repository;
        public AccountRepositoryTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _repository = new AccountRepository(_dbFixture.connection);
        }

        [Fact]
        public void GetAccountCountShouldReturnCountWhenExistingAccounts()
        {
            //Arrange
            _dbFixture.CleanAccountTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            int accountsCount = 2;

            foreach (int i in Enumerable.Range(0, accountsCount))
            {
                _dbFixture.connection.Execute($"INSERT INTO Account (Id, Name, CreatedDate) VALUES('{i}','Account{i}', '{now}')");
            }

            //Act
            var result = _repository.GetAccountCount();

            //Assert
            Assert.Equal(accountsCount, result);
        }

        [Fact]
        public void InsertAccountShouldReturnAddAccountToTable()
        {
            //Arrange
            _dbFixture.CleanAccountTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            int accountsCount = 3;

            //Act
            foreach (int i in Enumerable.Range(0, accountsCount))
            {
                _repository.InsertAccount(null, i, now);
            }

            //Assert
            int dbCount = _dbFixture.connection.QueryFirst<int>("SELECT COUNT(*) FROM Account;");
            Assert.Equal(accountsCount, dbCount);
        }
    }
}
