using Dapper;
using NSubstitute;
using SmartVault.BusinessLogic;
using SmartVault.BusinessLogic.BusinessObjects;
using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace SmartVault.Tests.Unit
{
    public class DocumentRepositoryTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture _dbFixture;
        IDocumentRepository _repository;
        private readonly IFileInfo _fileInfo = Substitute.For<IFileInfo>();
        public DocumentRepositoryTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _repository = new DocumentRepository();
        }

        [Fact]
        public void GetDocumentCountShouldReturnCountWhenExistingDocuments()
        {
            //Arrange
            _dbFixture.CleanDocumentTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            int documentsCount = 2;

            foreach (int i in Enumerable.Range(0, documentsCount))
            {
                _dbFixture.connection.Execute($"INSERT INTO Document (Name, FilePath, Length, AccountId, CreatedDate) VALUES('Document{i}.txt','D://Document{i}','200','{i}', '{now}')");
            }

            //Act
            var result = _repository.GetDocumentCount(_dbFixture.connection);

            //Assert
            Assert.Equal(documentsCount, result);
        }

        [Fact]
        public void GetDocumentPathListShouldReturnCollectionOfStrings()
        {
            //Arrange
            _dbFixture.CleanDocumentTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            int documentsCount = 3;

            foreach (int i in Enumerable.Range(0, documentsCount))
            {
                _dbFixture.connection.Execute($"INSERT INTO Document (Name, FilePath, Length, AccountId, CreatedDate) VALUES('Document{i}.txt','D://Document{i}','200','{i}', '{now}')");
            }

            //Act
            var result = _repository.GetDocumentPathList(_dbFixture.connection);

            //Assert
            Assert.All(result, i => Assert.IsType<string>(i));
            Assert.Equal(documentsCount, result.Count());
        }

        [Fact]
        public void GetDocumentPathListByAccountIdShouldReturnCollectionOfStrings()
        {
            //Arrange
            _dbFixture.CleanDocumentTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            _dbFixture.connection.Execute($"INSERT INTO Document (Name, FilePath, Length, AccountId, CreatedDate) VALUES('Document1.txt','D://Document1','200','1', '{now}')");
            _dbFixture.connection.Execute($"INSERT INTO Document (Name, FilePath, Length, AccountId, CreatedDate) VALUES('Document2.txt','D://Document2','200','2', '{now}')");

            //Act
            var result = _repository.GetDocumentPathListByAccountId(_dbFixture.connection, "1");


            //Assert
            Assert.All(result, i => Assert.IsType<string>(i));
            Assert.Single(result);
        }

        [Fact]
        public void InsertDocumentShouldReturnAddDocumentToTable()
        {
            //Arrange
            _dbFixture.CleanDocumentTableEntries();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            int documentsCount = 3;

            _fileInfo.Length.Returns(200);
            _fileInfo.FullName.Returns("Document");

            //Act
            foreach (int i in Enumerable.Range(0, documentsCount))
            {
                _repository.InsertDocument(_dbFixture.connection, null, i, i, _fileInfo, now);
            }

            //Assert
            int dbCount = _dbFixture.connection.QueryFirst<int>("SELECT COUNT(*) FROM Document;");
            Assert.Equal(documentsCount, dbCount);
        }
    }
}
