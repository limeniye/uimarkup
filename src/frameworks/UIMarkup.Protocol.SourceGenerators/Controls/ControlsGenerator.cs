using System.Extended;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace UIMarkup.Protocol.SourceGenerators;

[Generator]
public class ControlsGenerator : ISourceGenerator
{
	public void Initialize(GeneratorInitializationContext context) { }

	public void Execute(GeneratorExecutionContext context)
	{
		new Generator().Generate(context);
	}

	class Generator
	{
		private static readonly Dictionary<string, SyntaxTree> _cachedSyntaxTrees = new Dictionary<string, SyntaxTree>();

		internal void Generate(GeneratorExecutionContext context)
		{
			try
			{
				_cachedSyntaxTrees.Clear();
				// Get text for serialization
				var controlsJsonAdditionalFile = context.AdditionalFiles.FirstOrDefault();
				if (controlsJsonAdditionalFile == null)
				{
					return;
				}
				var controlsJsonAdditionalFilePath = controlsJsonAdditionalFile.Path;
				var sourceText = controlsJsonAdditionalFile.GetText();

				StringBuilder sbJson = new StringBuilder();
				foreach (var line in sourceText.Lines)
				{
					sbJson.AppendLine(line.ToString());
				}

				// Json serialization
				var controls = ControlsJsonSchemaGenerator.GenerateSchema(sbJson.ToString());

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
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception e)
			{
				string message = e.Message + e.StackTrace;

				if (e is AggregateException)
				{
					message = (e as AggregateException)?.InnerExceptions.Select(ex => ex.Message + e.StackTrace).JoinBy("\r\n");
				}

				var diagnostic = Diagnostic.Create(
					ControlsGeneratorDiagnostics.GenericProtocolErrorRule,
					null,
					$"Failed to generate type providers. ({e.Message})");

				context.ReportDiagnostic(diagnostic);
			}
		}

		private void AddSyntaxTreeWithCash(GeneratorExecutionContext context, string hintName, SyntaxTree syntaxTree)
		{
			var controlRecordText = syntaxTree.GetRoot().NormalizeWhitespace().ToFullString();

			context.AddSource($"{hintName}.g.cs", SourceText.From(controlRecordText, Encoding.UTF8));
			_cachedSyntaxTrees[hintName] = syntaxTree;
		}

		private static SyntaxTree CreateControlRecordSyntaxTree(string type, IReadOnlyDictionary<string, string> properties, GeneratorExecutionContext context)
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
	}
}
