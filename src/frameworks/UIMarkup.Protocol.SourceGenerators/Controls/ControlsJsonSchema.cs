using System.Collections.ObjectModel;
using System.Text.Json;

namespace UIMarkup.Protocol.SourceGenerators;

internal static class ControlsJsonSchemaGenerator
{
	public static IReadOnlyCollection<ControlJsonSchemaType> GenerateSchema(string json)
	{
		var schema = JsonSerializer.Deserialize<ControlsJsonSchema>(json, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		var validControls = schema.Controls
			.Select(control => control with
			{
				Properties = FilterInvalidProperties(control.Properties)
			});

		return new ReadOnlyCollection<ControlJsonSchemaType>(validControls.ToArray());
	}

	private static IReadOnlyDictionary<string, string> FilterInvalidProperties(IReadOnlyDictionary<string, string> properties)
	{
		var validProperties = new Dictionary<string, string>(properties.Count);
		foreach (var property in properties)
		{
			if (!property.Key.StartsWith("_"))
			{
				validProperties.Add(property.Key, property.Value);
			}
		}
		return validProperties;
	}
}

internal class ControlsJsonSchema
{
	public IReadOnlyCollection<ControlJsonSchemaType> Controls { get; init; } = Array.Empty<ControlJsonSchemaType>();
}

internal record ControlJsonSchemaType
{
	public string Type { get; init; } = string.Empty;

	public IReadOnlyDictionary<string, string> Properties { get; init; } = EmptyProperties;

	private static readonly IReadOnlyDictionary<string, string> EmptyProperties = new Dictionary<string, string>(0);
}
