using Microsoft.CodeAnalysis;

namespace UIMarkup.Protocol.SourceGenerators;

public static class ControlsGeneratorDiagnostics
{
	internal const string Title = "Control Generation Failed";
	internal const string MessageFormat = "{0}";
	internal const string Category = "Protocol";
	internal const string GenerationFailureDescription = "Control Generation Failed";
	public static readonly DiagnosticDescriptor GenericProtocolErrorRule = new(
#pragma warning disable RS2008 // Enable analyzer release tracking
		"UIM0001",
#pragma warning restore RS2008 // Enable analyzer release tracking
		Title,
		MessageFormat,
		Category,
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
#pragma warning disable RS1033 // Define diagnostic description correctly
		description: GenerationFailureDescription
#pragma warning restore RS1033 // Define diagnostic description correctly
		);
}
