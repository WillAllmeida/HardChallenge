using Dapper;
using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _repository = new AccountRepository();
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
            var result = _repository.GetAccountCount(_dbFixture.connection);

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
                _repository.InsertAccount(_dbFixture.connection, null, i, now);
            }

            //Assert
            int dbCount = _dbFixture.connection.QueryFirst<int>("SELECT COUNT(*) FROM Account;");
            Assert.Equal(accountsCount, dbCount);
        }
    }
}
