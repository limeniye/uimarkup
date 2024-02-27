namespace System.Extended.Reflection;

/// <summary>
/// Information about the type property and its value
/// </summary>
/// <param name="Source">The type that contains this property</param>
/// <param name="Property">Property type</param>
/// <param name="Value">Property value</param>
public sealed record PropertyValue(object Source, Type Property, object Value);
