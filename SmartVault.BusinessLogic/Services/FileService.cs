using SmartVault.BusinessLogic.Interfaces;
using System;
using System.IO;
using System.IO.Abstractions;

namespace SmartVault.BusinessLogic.Services
{
    public class FileService : IFileService
    {
        private readonly IFileSystem _fileSystem;

        private readonly string _directory = Path.GetFullPath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\..\\");

        public FileService() : this(new FileSystem()) { }

        public FileService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void CreateFile(string fileName, string content)
        {
            _fileSystem.File.WriteAllText(_directory + fileName, content);
        }

        public IFileInfo GetFileInfo(string fileName)
        {
            return _fileSystem.FileInfo.New(_directory + fileName);
        }

        public string CheckStringInFile(string path, string textToSearch)
        {
            string fileContent = _fileSystem.File.ReadAllText(path);

            if (fileContent.Contains(textToSearch))
            {
                return fileContent;
            }
            else
            {
                return "";
            }
        }

        public long GetFileSize(string path)
        {
            if (_fileSystem.File.Exists(path))
            {
                return _fileSystem.FileInfo.New(path).Length;
            }
            else
            {
                return 0;
            }
        }

        public bool VerifyIfFileExists(string fileName)
        {
            return _fileSystem.File.Exists(_directory + fileName);
        }
    }
}
