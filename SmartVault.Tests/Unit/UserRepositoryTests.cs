using Dapper;
using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Repositories;
using System;
using System.Linq;
using Xunit;

namespace SmartVault.Tests.Unit
{
    public class UserRepositoryTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture _dbFixture;
        IUserRepository _repository;
        public UserRepositoryTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _repository = new UserRepository(_dbFixture.connection);
        }

        [Fact]
        public void GetUserCountShouldReturnCountWhenExistingUsers()
        {
            //Arrange
            _dbFixture.CleanUserTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            int usersCount = 2;

            foreach (int i in Enumerable.Range(0, usersCount))
            {
                _dbFixture.connection.Execute($"INSERT INTO User (Id, FirstName, LastName, DateOfBirth, AccountId, Username, Password, CreatedDate) VALUES('{i}','FName{i}','LName{i}','{now}','{i}','UserName-{i}','e10adc3949ba59abbe56e057f20f883e', '{now}')");
            }

            //Act
            var result = _repository.GetUserCount();

            //Assert
            Assert.Equal(usersCount, result);
        }

        [Fact]
        public void InsertUserShouldReturnAddUserToTable()
        {
            //Arrange
            _dbFixture.CleanUserTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            int usersCount = 3;

            //Act
            foreach (int i in Enumerable.Range(0, usersCount))
            {
                _repository.InsertUser(null, i, now, now);
            }

            //Assert
            int dbCount = _dbFixture.connection.QueryFirst<int>("SELECT COUNT(*) FROM User;");
            Assert.Equal(usersCount, dbCount);
        }
    }
}
