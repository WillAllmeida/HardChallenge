using Dapper;
using SmartVault.BusinessLogic;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace SmartVault.Tests
{
    public class DatabaseFixture : DataFixture, IDisposable
    {
        public DatabaseHelper databaseHelper { get; set; }
        public SQLiteConnection connection { get; set; }

        public DatabaseFixture()
        {
            connection = new SQLiteConnection("Data Source=:memory:");
            databaseHelper = new DatabaseHelper();
            connection.Open();
            var files = _files.Select(t => _filesDirectory + t).ToArray();
            databaseHelper.CreateDatabaseTables(connection, files);
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
