using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartVault.BusinessLogic
{
    public class FileHelper
    {
        private readonly IFileSystem _fileSystem;

        private readonly string _directory = Path.GetFullPath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\..\\");

        public FileHelper() : this(new FileSystem()) { }

        public FileHelper(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        public void CreateFile(string fileName, string content)
        {
            _fileSystem.File.WriteAllText(_directory + fileName, content);
        }

        public IFileInfo GetFilePath(string fileName)
        {
            return _fileSystem.FileInfo.New(_directory + fileName);
        }

        public string CheckStringInFile(string path, string textToSearch)
        {
            string fileContent = _fileSystem.File.ReadAllText(path);
            //Melhorar
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
            var fileSize = _fileSystem.FileInfo.New(path).Length;
            return fileSize;
        }

        public bool VerifyIfFileExists(string fileName)
        {
            return File.Exists(_directory + fileName);
        }
    }
}
