using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartVault.BusinessLogic
{
    public class FileHelper
    {
        private readonly string _directory = Path.GetFullPath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\..\\");
        public void CreateFile(string fileName, string content)
        {
            File.WriteAllText(_directory + fileName, content);
        }

        public FileInfo GetFilePath(string fileName)
        {
            return new FileInfo(_directory + fileName);
        }

        public string CheckStringInFile(string path, string textToSearch)
        {
            string fileContent = File.ReadAllText(path);
            if (fileContent.Contains(textToSearch))
            {
                return fileContent;
            }
            else
            {
                return "";
            }
        }
    }
}
