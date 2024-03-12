using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
namespace UIMarkup.Protocol.SourceGenerators;

[Generator]
public class ControlGenerator : ISourceGenerator
{
	private static readonly Dictionary<string, SyntaxTree> _cachedSyntaxTrees = new Dictionary<string, SyntaxTree>();
	private GeneratorExecutionContext _context;
	private Dictionary<string, JsonFileMonitor> _jsonFileMonitors = new();

	public void Execute(GeneratorExecutionContext context)
	{
		this._context = context;
		_cachedSyntaxTrees.Clear();
		// Get text for serialization
		var controlsJsonAdditionalFile = context.AdditionalFiles.First();
		var controlsJsonAdditionalFilePath = controlsJsonAdditionalFile.Path;
		if (!_jsonFileMonitors.ContainsKey(controlsJsonAdditionalFilePath))
		{
			var jsonFileMonitoring = new JsonFileMonitor(controlsJsonAdditionalFilePath, this);
			_jsonFileMonitors.Add
				(
					controlsJsonAdditionalFilePath,
					jsonFileMonitoring
				);
		}
		var sourceText = controlsJsonAdditionalFile.GetText();

		StringBuilder sbJson = new StringBuilder();
		foreach (var line in sourceText.Lines)
		{
			sbJson.AppendLine(line.ToString());
		}

		// Json serialization
		var controlsSchemaGenerator = new ControlsJsonSchemaGenerator();
		var controls = controlsSchemaGenerator.GenerateSchema(sbJson.ToString());

		if (controls.Count == 0)
		{
			return;
		}

		foreach (var control in controls)
		{
			var controlRecordSyntaxTree = CreateControlRecordSyntaxTree(control.Type, control.Properties, context);
			AddSyntaxTreeWithCash(context, $"{control.Type.Replace(" ", "").Split(':')[0]}", controlRecordSyntaxTree);
		}
	}

	public void Execute()
	{
		Execute(_context);
	}

	private void AddSyntaxTreeWithCash(GeneratorExecutionContext context, string hintName, SyntaxTree syntaxTree)
	{
		var controlRecordText = syntaxTree.GetRoot().NormalizeWhitespace().ToFullString();

		context.AddSource($"{hintName}.g.cs", SourceText.From(controlRecordText, Encoding.UTF8));
		_cachedSyntaxTrees[hintName] = syntaxTree;
	}

	private static SyntaxTree CreateControlRecordSyntaxTree(string type, Dictionary<string, string> properties, GeneratorExecutionContext context)
	{
		string[] typeAndInherited = type.Replace(" ", "").Split(':');
		var parameters = SyntaxFactory.SeparatedList(
			properties.Select(x =>
				SyntaxFactory.Parameter(SyntaxFactory.Identifier(x.Key))
				.WithType(SyntaxFactory.ParseTypeName(x.Value))
				.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed)
				)
			);

		var recordDeclaration =
			SyntaxFactory.RecordDeclaration(
				SyntaxFactory.Token(SyntaxKind.RecordKeyword),
				typeAndInherited[0]
			)
			.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

		// Inheritance
		if (typeAndInherited.Length > 1)
		{
			var inheritedTypeSyntax = _cachedSyntaxTrees[typeAndInherited[1]];
			var inheritedRoot = inheritedTypeSyntax.GetRoot();
			var inheritedRecordDeclaration = inheritedRoot.DescendantNodes().OfType<RecordDeclarationSyntax>().FirstOrDefault();
			var inheritedParametersSyntax = inheritedRecordDeclaration.ParameterList.Parameters;
			var inheritedArgumentList =
				SyntaxFactory.SeparatedList(
					inheritedParametersSyntax.Select(
						p => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(p.Identifier))
						)
					);
			var recordAndInheritedParameters = parameters.AddRange(inheritedParametersSyntax);

			recordDeclaration = recordDeclaration
				.WithParameterList(SyntaxFactory.ParameterList(recordAndInheritedParameters))
				.AddBaseListTypes(SyntaxFactory.SimpleBaseType(
					SyntaxFactory.ParseTypeName($"{typeAndInherited[1]}({inheritedArgumentList})"))
				);
		}
		else
		{
			recordDeclaration = recordDeclaration
						.WithParameterList(SyntaxFactory.ParameterList(parameters));
		}

		SyntaxTree tree = SyntaxFactory.SyntaxTree(
			SyntaxFactory.CompilationUnit().AddMembers(
				SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("UIMarkup.Protocol"))
				.AddMembers(
					recordDeclaration
					.WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
					.WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken))
					)
			)
		);
		return tree;
	}

	public void Initialize(GeneratorInitializationContext context) { }
}

internal class ControlsJsonSchema
{
	public IReadOnlyCollection<ControlJsonSchemaType> Controls { get; init; } = Array.Empty<ControlJsonSchemaType>();
}

internal class ControlJsonSchemaType
{
	public string Type { get; init; } = string.Empty;
	public Dictionary<string, string> Properties { get; init; }
}

internal class Property
{
	public string Name { get; init; } = string.Empty;
	public string Type { get; init; } = string.Empty;
}

internal class ControlsJsonSchemaGenerator
{
	public IReadOnlyCollection<ControlJsonSchemaType> GenerateSchema(string json)
	{
		var schema = JsonSerializer.Deserialize<ControlsJsonSchema>(json, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		foreach (var control in schema.Controls)
		{
			var propertiesToRemove = control.Properties.Keys.Where(key => key.StartsWith("_"));
			foreach (var propertyToRemove in propertiesToRemove)
			{
				control.Properties.Remove(propertyToRemove);
			}
		}

		return schema.Controls;
	}
}
