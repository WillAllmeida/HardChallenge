using System.Collections.Generic;
using System.Data.SQLite;
using System.IO.Abstractions;

namespace SmartVault.BusinessLogic.Interfaces
{
    public interface IDocumentRepository
    {
        IEnumerable<string> GetDocumentPathListByAccountId(string accountId);
        IEnumerable<string> GetDocumentPathList();
        void InsertDocument(SQLiteTransaction? transaction, int id, int documentCount, IFileInfo documentInfo, string currentDate);
        int GetDocumentCount();
    }
}
