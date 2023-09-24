using Dapper;
using NSubstitute;
using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Services;
using System;
using System.Data.SQLite;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace SmartVault.Tests
{
    public class DatabaseFixture : DataFixture, IDisposable
    {
        public IDatabaseService databaseService { get; set; }
        public SQLiteConnection connection { get; set; }
        public IFileInfo fileInfo;

        public DatabaseFixture()
        {
            connection = new SQLiteConnection("Data Source=:memory:");
            databaseService = new DatabaseService();
            fileInfo = Substitute.For<IFileInfo>();
            connection.Open();
            var files = _files.Select(t => _filesDirectory + t).ToArray();
            databaseService.CreateDatabaseTables(connection, files);
        }

        public void DropAllTables()
        {
            Array.ForEach(_files, f =>
            {
                connection.Execute($"DROP TABLE IF EXISTS {Path.GetFileNameWithoutExtension(f)}");
            });
        }

        public int GetTablesCount()
        {
            return connection.QueryFirst<int>("SELECT count(*) FROM sqlite_master WHERE type = 'table'");
        }

        public void CleanAccountTableEntries()
        {
            connection.Execute($"delete from Account");
        }

        public void CleanUserTableEntries()
        {
            connection.Execute($"delete from User");
        }

        public void CleanOAuthUserTableEntries()
        {
            connection.Execute($"delete from OAuthUser");
        }

        public void CleanDocumentTableEntries()
        {
            connection.Execute($"delete from Document");
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
