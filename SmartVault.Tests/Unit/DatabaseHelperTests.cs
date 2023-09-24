using Dapper;
using SmartVault.BusinessLogic;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace SmartVault.Tests.Unit
{
    public class DatabaseHelperTests : DataFixture, IClassFixture<DatabaseFixture>
    {
        DatabaseFixture _dbFixture;
        public DatabaseHelperTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
        }

        [Fact]
        public void CreateDatabaseTablesShouldCreateOneTableForEachFile()
        {
            //Arrange
            var files = _files.Select(t => _filesDirectory + t).ToArray();
            _dbFixture.DropAllTables();

            //Act
            _dbFixture.databaseHelper.CreateDatabaseTables(_dbFixture.connection, files);

            //Assert
            var tablesCount = _dbFixture.GetTablesCount();
            var everyTableExists = _files.Select(f => _dbFixture.connection.QueryFirst<string>($"SELECT name FROM sqlite_master WHERE type='table' AND name='{Path.GetFileNameWithoutExtension(f)}';") == Path.GetFileNameWithoutExtension(f));

            Assert.Equal(_files.Length, tablesCount);
            Assert.All(everyTableExists, i => Assert.True(i));
        }

        [Fact]
        public void CreateDatabaseTablesShouldNotCreateTablesWhenNoFilesProvided()
        {
            //Arrange
            _dbFixture.DropAllTables();

            //Act
            _dbFixture.databaseHelper.CreateDatabaseTables(_dbFixture.connection, new string[0]);

            //Assert
            var tablesCount = _dbFixture.GetTablesCount();
            Assert.Equal(0, tablesCount);
        }
    }
}
