using Dapper;
using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Repositories;
using System;
using System.Linq;
using Xunit;

namespace SmartVault.Tests.Unit
{
    public class OAuthUserRepositoryTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture _dbFixture;
        IOAuthUserRepository _repository;
        public OAuthUserRepositoryTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _repository = new OAuthUserRepository(_dbFixture.connection);
        }

        [Fact]
        public void GetOAuthUserCountShouldReturnCountWhenExistingOAuthUsers()
        {
            //Arrange
            _dbFixture.CleanOAuthUserTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string email = "teste@teste.com";
            string token = new Guid().ToString();
            string provider = "Google";
            int usersCount = 2;

            foreach (int i in Enumerable.Range(0, usersCount))
            {
                _dbFixture.connection.Execute($"INSERT INTO OAuthUser (UserId, Email, AccessToken, RefreshToken, Provider, TokenExpiration, CreatedDate) VALUES('{i}','{email}','{token}','{token}','{provider}','{now}', '{now}')");
            }

            //Act
            var result = _repository.GetOAuthUserCount();

            //Assert
            Assert.Equal(usersCount, result);
        }
        [Fact]
        public void InsertOAuthUserShouldReturnAddOAuthUserToTable()
        {
            //Arrange
            _dbFixture.CleanOAuthUserTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string token = new Guid().ToString();
            string email = "test@test.com";
            string provider = "Google";
            int usersCount = 3;

            //Act
            foreach (int i in Enumerable.Range(0, usersCount))
            {
                _repository.InsertOAuthUser(null, i, email, token, provider, now);
            }

            //Assert
            int dbCount = _dbFixture.connection.QueryFirst<int>("SELECT COUNT(*) FROM OAuthUser;");
            Assert.Equal(usersCount, dbCount);
        }
    }
}
