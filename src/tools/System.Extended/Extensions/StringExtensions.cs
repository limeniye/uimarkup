namespace System.Extended;
internal static class StringExtensions
{
	public static string JoinBy(this IEnumerable<string> items, string joinBy)
	{
		return string.Join(joinBy, items);
	}
}
