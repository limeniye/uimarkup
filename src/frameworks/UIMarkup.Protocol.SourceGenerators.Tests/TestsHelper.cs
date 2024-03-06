using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;

namespace UIMarkup.Protocol.SourceGenerators.Tests;

public static class TestsHelper
{
	public static ImmutableArray<AdditionalText> GetAdditionalText(string path)
	{
		var filePath = $"{Environment.CurrentDirectory}\\{path}";
		var additional = new List<AdditionalText>();
		if (path.Contains('*'))
		{
			throw new NotImplementedException();
		}
		else
		{
			var fileText = File.ReadAllText(filePath);
			additional.Add(new TestAdditionalFile(filePath, fileText));
		}
		return additional.ToImmutableArray();
	}

	public static GeneratorDriver RunGeneratorDriver(
		this ISourceGenerator generator,
		ImmutableArray<AdditionalText>? additional = null,
		IEnumerable<PortableExecutableReference> references = null)
	{
		var compilation = CSharpCompilation.Create(
			assemblyName: "UIMarkup.Protocol.SourceGenerators",
			references: references);

		var driver = CSharpGeneratorDriver.Create(generator);
		if (additional != null)
		{
			return driver
				.AddAdditionalTexts((ImmutableArray<AdditionalText>)additional)
				.RunGenerators(compilation);
		}
		return driver.RunGenerators(compilation);
	}
}
