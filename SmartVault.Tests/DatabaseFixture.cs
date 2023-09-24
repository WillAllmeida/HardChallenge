using System;
using System.Data.SQLite;

namespace SmartVault.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public SQLiteConnection connection { get; set; }

        public DatabaseFixture()
        {
            connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
