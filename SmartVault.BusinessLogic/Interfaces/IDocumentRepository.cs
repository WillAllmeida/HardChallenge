using System.Collections.Generic;
using System.Data.SQLite;
using System.IO.Abstractions;

namespace SmartVault.BusinessLogic.Interfaces
{
    public interface IDocumentRepository
    {
        IEnumerable<string> GetDocumentPathListByAccountId(SQLiteConnection connection, string accountId);
        IEnumerable<string> GetDocumentPathList(SQLiteConnection connection);
        void InsertDocument(SQLiteConnection connection, SQLiteTransaction? transaction, int id, int documentCount, IFileInfo documentInfo, string currentDate);
        int GetDocumentCount(SQLiteConnection connection);
    }
}
