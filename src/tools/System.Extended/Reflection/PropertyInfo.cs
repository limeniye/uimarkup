namespace System.Extended.Reflection;

/// <summary>
/// Information about the type property
/// </summary>
/// <param name="SourceType">The type that contains this property</param>
/// <param name="Property">Property type</param>
/// <param name="Name">Property name</param>
/// <remarks>
/// Used in generated types
/// </remarks>
public sealed record PropertyInfo(Type SourceType, Type Property, string Name);
