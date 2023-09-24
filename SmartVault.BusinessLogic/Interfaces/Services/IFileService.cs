using System.IO.Abstractions;

namespace SmartVault.BusinessLogic.Interfaces
{
    public interface IFileService
    {
        void CreateFile(string fileName, string content);
        IFileInfo GetFileInfo(string fileName);
        string CheckStringInFile(string path, string textToSearch);
        long GetFileSize(string path);
        bool VerifyIfFileExists(string fileName);
    }
}
