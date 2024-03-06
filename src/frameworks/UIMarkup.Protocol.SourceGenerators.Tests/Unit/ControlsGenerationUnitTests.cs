using Microsoft.CodeAnalysis;

namespace UIMarkup.Protocol.SourceGenerators.Tests.Unit;


public class ControlsGenerationUnitTests
{
	[Fact]
	public Task ControlGenerator_GenerateUIElement_Verify()
	{
		// arrange
		var controlsGenerator = new ControlGenerator();
		var additional = TestsHelper.GetAdditionalText("Stubs\\testProtocolControls.json");
		var references = new[]
		{
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(HorizontalAlignment).Assembly.Location)
		};

		// act
		var driver = controlsGenerator.RunGeneratorDriver(additional, references);
		//var runResult = driver.GetRunResult().Results.Single();

		// assert
		return Verify(driver).UseDirectory("Snapshots");
	}

	//private static GeneratedCode? GetGeneratedOutput(
	//  string sourceCode,
	//  params AdditionalText[] additionalFiles)
	//{
	//	var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
	//	var references = AppDomain.CurrentDomain.GetAssemblies()
	//							  .Where(assembly => !assembly.IsDynamic)
	//							  .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
	//							  .Cast<MetadataReference>();

	//	var compilation = CSharpCompilation.Create("SourceGeneratorTests",
	//											   new[] { syntaxTree },
	//											   references,
	//											   new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
	//	var generator = new ControlGenerator();
	//	CSharpGeneratorDriver.Create(generator)
	//						 .AddAdditionalTexts(additionalFiles.ToImmutableArray())
	//						 .RunGeneratorsAndUpdateCompilation(compilation,
	//															out var outputCompilation,
	//															out var diagnostics);

	//	// optional
	//	diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)
	//			   .Should().BeEmpty();

	//	if (outputCompilation.SyntaxTrees.Count() != 4)
	//		return null;

	//	return new GeneratedCode(outputCompilation.SyntaxTrees.Single(t => t.FilePath.Contains(".Main.g.cs")).ToString(),
	//							 outputCompilation.SyntaxTrees.Single(t => t.FilePath.Contains(".Translations.g.cs")).ToString(),
	//							 outputCompilation.SyntaxTrees.Single(t => t.FilePath.Contains(".NewtonsoftJson.g.cs")).ToString());
	//}
}
