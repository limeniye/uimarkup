using System.Text.Json;
using System.Text;
using Microsoft.CodeAnalysis;

namespace UIMarkup.Protocol.SourceGenerators;

[Generator]
public class ControlGenerator : ISourceGenerator
{
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
		var schema = JsonSerializer.Deserialize<ControlsJsonSchema>(sbJson.ToString(), new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		if (schema.Controls.Count == 0)
		{
			return;
		}

		// Name and Code
		List<(string TypeName, string TypeCode)> controlRecordsCode = new List<(string TypeName, string TypeCode)>(schema.Controls.Count);
		foreach (var control in schema.Controls)
		{
			StringBuilder sbProperties = new StringBuilder();
			var properties = control.Properties;
			int i = 1;
			foreach (var property in properties)
			{
				sbProperties.Append($"\t{property.Value} {property.Key}");
				if (i != properties.Count)
				{
					sbProperties.AppendLine($",");
					i++;
				}
			}
			string controlRecordCode =
$@"namespace UIMarkup.Protocol;

public record {control.Type}(
	Guid Guid{(sbProperties.Length > 0 ? ",\n" : string.Empty)}{sbProperties})
{{
}}
";
			controlRecordsCode.Add((control.Type, controlRecordCode));
		}

		foreach (var controlRecord in controlRecordsCode)
		{
			context.AddSource($"{controlRecord.TypeName}.g.cs", controlRecord.TypeCode);
		}
	}

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
