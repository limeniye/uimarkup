namespace UIMarkup.Protocol;

/// <param name="Owner">An element that has information about a column</param>
/// <param name="Value">Could be Nan</param>
/// <param name="ContainerSideLength">Explains how <see cref="Value"/> was calculated</param>
public record struct Column(string Name, IElement Owner, double Value, ContainerSideLength ContainerSideLength);
