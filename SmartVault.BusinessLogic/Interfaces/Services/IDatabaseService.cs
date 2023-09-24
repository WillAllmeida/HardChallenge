using System.Data.SQLite;

namespace SmartVault.BusinessLogic.Interfaces
{
    public interface IDatabaseService
    {
        SQLiteConnection GetDatabaseConnection();
        void CreateDatabaseFile();
        void CreateDatabaseTables(SQLiteConnection connection, string[] files);
        string GetDatabaseName();
    }
}
