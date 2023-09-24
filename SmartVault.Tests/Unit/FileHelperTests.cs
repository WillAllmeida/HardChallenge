using NSubstitute;
using System;
using System.IO;
using System.IO.Abstractions;
using Xunit;
using FileHelper = SmartVault.BusinessLogic.FileHelper;

namespace SmartVault.Tests.Unit
{
    public class FileHelperTests : DataFixture
    {
        private readonly IFileSystem _mock = Substitute.For<IFileSystem>();
        private readonly IFileInfo _fileInfo = Substitute.For<IFileInfo>();
        private readonly FileHelper _fileHelper;

        public FileHelperTests()
        {
            _fileHelper = new FileHelper(_mock);
        }

        [Fact]
        public void VerifyIfFileExistsShouldReturnTrueWhenValidInput()
        {
            //Arrange
            string filename = "test.txt";

            _mock.File.Exists(_directory + filename).Returns(true);


            //Act
            _mock.ClearReceivedCalls();
            var result = _fileHelper.VerifyIfFileExists(filename);

            //Assert
            Assert.True(result);
            _mock.Received(1).File.Exists(_directory + filename).ReturnsForAnyArgs(true);
        }

        [Fact]
        public void VerifyIfFileExistsShouldReturnTrueWhenInvalidInput()
        {
            //Arrange
            string filename = "invalidtest.txt";

            _mock.File.Exists(Arg.Any<string>()).Returns(false);

            //Act
            _mock.ClearReceivedCalls();
            var result = _fileHelper.VerifyIfFileExists(filename);

            //Assert
            Assert.False(result);
            _mock.Received(1).File.Exists(filename).ReturnsForAnyArgs(false);
        }

        [Fact]
        public void GetFileSizeShouldReturnSizeWhenValidInput()
        {
            //Arrange
            string filename = "existingtest.txt";
            long length = 500;

            _fileInfo.Length.Returns(length);

            _mock.File.Exists(filename).Returns(true);
            _mock.FileInfo.New(filename).Returns(_fileInfo);

            //Act
            _mock.ClearReceivedCalls();
            var result = _fileHelper.GetFileSize(filename);

            //Assert
            Assert.Equal(length, result);
            _mock.Received(1).File.Exists(filename).ReturnsForAnyArgs(true);
            _mock.Received(1).FileInfo.New(filename).ReturnsForAnyArgs(_fileInfo);
        }

        [Fact]
        public void GetFileSizeShouldReturnZeroWhenInvalidInput()
        {
            //Arrange
            string filename = "invalidtest.txt";

            _mock.File.Exists(Arg.Any<string>()).Returns(false);

            //Act
            _mock.ClearReceivedCalls();
            var result = _fileHelper.GetFileSize(filename);

            //Assert
            Assert.Equal(0, result);
            _mock.Received(1).File.Exists(filename).ReturnsForAnyArgs(false);
            _mock.DidNotReceive().FileInfo.New(Arg.Any<string>());
        }

        [Fact]
        public void GetFileInfoShouldReturnNewFileInfoWhenValidFileName()
        {
            //Arrange
            string filename = "existingtest.txt";
            var fileData = filename.Split(".");
            long length = 500;

            _fileInfo.Length.Returns(length);
            _fileInfo.Extension.Returns(fileData[1]);
            _fileInfo.Name.Returns(fileData[0]);

            _mock.FileInfo.New(_directory + filename).Returns(_fileInfo);

            //Act
            _mock.ClearReceivedCalls();
            var result = _fileHelper.GetFileInfo(filename);

            //Assert
            Assert.Equal(length, result.Length);
            Assert.Equal(fileData[0], result.Name);
            Assert.Equal(fileData[1], result.Extension);

            _mock.Received(1).FileInfo.New(_directory + filename).ReturnsForAnyArgs(_fileInfo);
        }

        [Fact]
        public void CreateFileShouldCallCorrectMethods()
        {
            //Arrange
            string filename = "newfile.txt";
            string content = "testcontent";

            _mock.File.WriteAllText(filename, content);

            //Act
            _mock.ClearReceivedCalls();
            _fileHelper.CreateFile(filename, content);

            //Assert
            _mock.Received(1).File.WriteAllText(filename, content);
        }

        [Fact]
        public void CheckStringInFileShouldReturnContentWhenTextMatched()
        {
            //Arrange
            string filename = "newfile.txt";
            string content = "test string in a new file";
            string textToSearch = "new";

            _mock.File.ReadAllText(filename).Returns(content);

            //Act
            _mock.ClearReceivedCalls();
            string result = _fileHelper.CheckStringInFile(filename, textToSearch);

            //Assert
            Assert.Equal(content, result);
            _mock.Received(1).File.ReadAllText(filename).ReturnsForAnyArgs(content);
        }

        [Fact]
        public void CheckStringInFileShouldReturnEmptyWhenTextNotMatched()
        {
            //Arrange
            string filename = "newfile.txt";
            string content = "random string";
            string textToSearch = "test";

            _mock.File.ReadAllText(filename).Returns(content);

            //Act
            _mock.ClearReceivedCalls();
            string result = _fileHelper.CheckStringInFile(filename, textToSearch);

            //Assert
            Assert.Equal(string.Empty, result);
            _mock.Received(1).File.ReadAllText(filename).ReturnsForAnyArgs(content);
        }
    }
}
