using SmartVault.BusinessLogic;
using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SmartVault.Program
{
    partial class Program
    {
        static DatabaseHelper _databaseHelper = new DatabaseHelper();
        static IDocumentRepository _documentRepository = new DocumentRepository();
        static FileHelper _fileHelper = new FileHelper();

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                GetAllFileSizes();
                WriteEveryThirdFileToFile("3");
                return;
            }

        }

        private static void GetAllFileSizes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            long totalSize = 0;
            int totalFiles = 0;
            var pathCache = new Dictionary<string, long>();

            using (var connection = _databaseHelper.GetDatabaseConnection())
            {
                var queryResult = _documentRepository.GetDocumentPathList(connection);

                foreach (var path in queryResult)
                {
                    totalFiles += 1;
                    if (pathCache.TryGetValue(path, out long currentSize))
                    {
                        totalSize += currentSize;
                    }
                    else
                    {
                        long size = _fileHelper.GetFileSize(path);
                        pathCache[path] = size;
                        totalSize += size;
                    }
                }
            }

            Console.WriteLine($"Total Size of {totalFiles} files: " + totalSize);
        }

        private static void WriteEveryThirdFileToFile(string accountId, string outputFile = "result.txt")
        {
            var sb = new StringBuilder();
            using (var connection = _databaseHelper.GetDatabaseConnection())
            {
                var queryResult = _documentRepository.GetDocumentPathListByAccountId(connection, accountId);

                for (int i = 0; i < queryResult?.Count() / 3; i = i + 3)
                {
                    sb.Append(_fileHelper.CheckStringInFile(queryResult?.ElementAt(i), "Smith Property"));
                }

                _fileHelper.CreateFile(outputFile, sb.ToString());
                Console.WriteLine($"The new file {outputFile} has been created");
            }
        }
    }
}