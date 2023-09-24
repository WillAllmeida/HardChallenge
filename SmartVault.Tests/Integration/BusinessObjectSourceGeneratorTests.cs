using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using SmartVault.CodeGeneration;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Xunit;

namespace SmartVault.Tests.Integration
{
    public class BusinessObjectSourceGeneratorTests : DataFixture
    {

        [Fact]
        public void ExecuteSourceGeneratorShouldCreateOneClassForEachXMLFile()
        {
            //Arrange
            Compilation inputCompilation = CreateCompilation(@"
            class Test 
            {
            }
            ");
            BusinessObjectSourceGenerator generator = new BusinessObjectSourceGenerator();

            var files = _files.Select(t => new CustomAdditionalText(_filesDirectory + t)).ToArray();
            var additionalTexts = ImmutableArray.Create<AdditionalText>(files);

            GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] { generator }, additionalTexts: additionalTexts);

            //Act
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
            GeneratorDriverRunResult runResult = driver.GetRunResult();


            //Assert
            Debug.Assert(diagnostics.IsEmpty);
            Debug.Assert(runResult.GeneratedTrees.Length == _files.Length);
            Debug.Assert(runResult.Diagnostics.IsEmpty);
        }
        private static Compilation CreateCompilation(string source)
        => CSharpCompilation.Create("SmartVault.Tests",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }


    internal class CustomAdditionalText : AdditionalText
    {
        private readonly string _text;
        public override string Path { get; }

        internal CustomAdditionalText(string path)
        {
            Path = path;
            _text = File.ReadAllText(path);
        }

        public override SourceText GetText(CancellationToken cancellationToken = new CancellationToken())
        {
            return SourceText.From(_text);
        }
    }
}
