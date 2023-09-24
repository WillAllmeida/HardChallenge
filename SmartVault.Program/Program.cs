using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Repositories;
using SmartVault.BusinessLogic.Services;
using System;
using System.Collections.Concurrent;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartVault.Program
{
    partial class Program
    {
        static SQLiteConnection _connection = null!;
        static IDocumentRepository _documentRepository = null!;
        static IDatabaseService _databaseService = new DatabaseService();
        static IFileService _fileService = new FileService();

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            WriteEveryThirdFileToFile(args[0]);
            GetAllFileSizes();
        }
        private static void GetAllFileSizes()
        {
            long totalSize = 0;
            int totalFiles = 0;
            var pathCache = new ConcurrentDictionary<string, long>();

            using (_connection = _databaseService.GetDatabaseConnection())
            {
                InitializeRepositories();
                var queryResult = _documentRepository.GetDocumentPathList();

                Parallel.ForEach(queryResult, path =>
                {
                    Interlocked.Increment(ref totalFiles);

                    if (pathCache.TryGetValue(path, out long currentSize))
                    {
                        Interlocked.Add(ref totalSize, currentSize);
                    }
                    else
                    {
                        long size = _fileService.GetFileSize(path);
                        pathCache[path] = size;
                        Interlocked.Add(ref totalSize, size);
                    }
                });
            }

            Console.WriteLine($"Total Size of {totalFiles} files: " + totalSize);
        }

        private static void WriteEveryThirdFileToFile(string accountId, string outputFile = "result.txt", string textToSearch = "Smith Property")
        {
            var sb = new StringBuilder();
            using (_connection = _databaseService.GetDatabaseConnection())
            {
                InitializeRepositories();

                var queryResult = _documentRepository.GetDocumentPathListByAccountId(accountId);

                for (int i = 0; i < queryResult?.Count() / 3; i = i + 3)
                {
                    sb.Append(_fileService.CheckStringInFile(queryResult?.ElementAt(i), textToSearch));
                }

                _fileService.CreateFile(outputFile, sb.ToString());
                Console.WriteLine($"The new file {outputFile} has been created");
            }
        }

        private static void InitializeRepositories()
        {
            _documentRepository = new DocumentRepository(_connection);
        }
    }
}