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

	public void Execute(GeneratorExecutionContext context)
	{
		// Get text for serialization
		var schemaJson = context.AdditionalFiles.First();
		var sourceText = schemaJson.GetText();

		StringBuilder sbJson = new StringBuilder();
		foreach (var line in sourceText.Lines)
		{
			sbJson.AppendLine(line.ToString());
		}

		// Json serialization
		var controls = GetControlsFromJson(sbJson.ToString());

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

	private void AddSyntaxTreeWithCash(GeneratorExecutionContext context, string hintName, SyntaxTree syntaxTree)
	{
		var controlRecordText = syntaxTree.GetRoot().NormalizeWhitespace().ToFullString();

		context.AddSource($"{hintName}.g.cs", SourceText.From(controlRecordText, Encoding.UTF8));
		_cachedSyntaxTrees.Add(hintName, syntaxTree);
	}

	private static IReadOnlyCollection<ControlJsonSchemaType> GetControlsFromJson(string json)
	{
		var schema = JsonSerializer.Deserialize<ControlsJsonSchema>(json, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});
		return schema.Controls;
	}

	private static SyntaxTree CreateControlRecordSyntaxTree(string type, Dictionary<string, string> properties, GeneratorExecutionContext context)
	{
		string[] typeAndInherited = type.Replace(" ", "").Split(':');
		var parameters = SyntaxFactory.SeparatedList(
			properties.Select(x =>
				SyntaxFactory.Parameter(SyntaxFactory.Identifier(x.Key))
				.WithType(SyntaxFactory.ParseTypeName(x.Value))
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

			//NameColonSyntax typeName = SyntaxFactory.NameColon(SyntaxFactory.IdentifierName(typeAndInherited[1]));
			//ConstructorInitializerSyntax initializer = SyntaxFactory.ConstructorInitializer(
			//	SyntaxKind.ThisConstructorInitializer,
			//	SyntaxFactory.Token(SyntaxKind.ColonToken),
			//	SyntaxFactory.ParseToken(typeAndInherited[1]),
			//	SyntaxFactory.ArgumentList(inheritedArgumentList));

			//var inheritedConstructor = SyntaxFactory.ConstructorInitializer(
			//	SyntaxKind.ThisConstructorInitializer,
			//	SyntaxFactory.Token(SyntaxKind.ColonToken),
			//	SyntaxFactory.ArgumentList(test),
			//	SyntaxFactory.IdentifierName(typeAndInherited[1])
			//	);

			recordDeclaration = recordDeclaration
				.WithParameterList(SyntaxFactory.ParameterList(inheritedParametersSyntax))
				//.WithBaseList(test2)
				//.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(typeAndInherited[1])))
				.AddMembers(
					SyntaxFactory.ConstructorDeclaration(typeAndInherited[1])
					.WithInitializer(
						SyntaxFactory.ConstructorInitializer(
							SyntaxKind.BaseConstructorInitializer,
							//SyntaxFactory.IdentifierName(typeAndInherited[1]),
							SyntaxFactory.ArgumentList(inheritedArgumentList)
							)
						)
				)
				//.AddMembers(SyntaxFactory.ConstructorDeclaration(typeAndInherited[1])
				//.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
				//.WithParameterList(SyntaxFactory.ParameterList(inheritedParametersSyntax))
				;

			//recordDeclaration = recordDeclaration.WithBaseList(
			//	SyntaxFactory.BaseList(
			//		SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
			//			SyntaxFactory.SimpleBaseType(
			//				SyntaxFactory.ParseTypeName(typeAndInherited[1])
			//			)
			//		)
			//	)
			//);

			//var derivedConstructor = SyntaxFactory.ConstructorDeclaration(typeAndInherited[0])
			//	.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
			//	.WithParameterList(SyntaxFactory.ParameterList(inheritedParametersSyntax))
			//	.WithInitializer(
			//		SyntaxFactory.ConstructorInitializer(
			//			SyntaxKind.BaseConstructorInitializer,
			//			SyntaxFactory.ArgumentList(
			//				SyntaxFactory.SeparatedList(
			//					inheritedParametersSyntax.Select(parameter =>
			//						SyntaxFactory.Argument(SyntaxFactory.IdentifierName(parameter.Identifier))
			//					)
			//				)
			//			)
			//		)
			//	);
			//recordDeclaration = recordDeclaration.AddMembers(derivedConstructor);
		}
		else
		{
			recordDeclaration = recordDeclaration
						.WithParameterList(SyntaxFactory.ParameterList(parameters))
						.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
		}

		SyntaxTree tree = SyntaxFactory.SyntaxTree(
			SyntaxFactory.CompilationUnit().AddMembers(
				SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("UIMarkup.Protocol"))
				.AddMembers(recordDeclaration)
			)
		);
		return tree;
	}

	/*
	// Створення синтаксичного дерева
			SyntaxTree tree = SyntaxFactory.SyntaxTree(
				SyntaxFactory.CompilationUnit()
					.AddUsings(
						SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"))
					)
					.AddMembers(
						SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("MyNamespace"))
							.AddMembers(
								SyntaxFactory.ClassDeclaration("MyClass")
									.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
									.AddMembers(
										SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "MyMethod")
											.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
											.WithBody(SyntaxFactory.Block())
									)
							)
					)
			);

			// Збереження синтаксичного дерева в файлі
			using (var fileStream = new FileStream("MyFile.cs", FileMode.Create))
			{
				tree.GetRoot().NormalizeWhitespace().WriteTo(fileStream);
			}
	 */

	public void Initialize(GeneratorInitializationContext context) { }
}

public class ControlsJsonSchema
{
	public IReadOnlyCollection<ControlJsonSchemaType> Controls { get; init; } = Array.Empty<ControlJsonSchemaType>();
}

public class ControlJsonSchemaType
{
	public string Type { get; init; } = string.Empty;
	public Dictionary<string, string> Properties { get; init; }
}

public class Property
{
	public string Name { get; init; } = string.Empty;
	public string Type { get; init; } = string.Empty;
}
