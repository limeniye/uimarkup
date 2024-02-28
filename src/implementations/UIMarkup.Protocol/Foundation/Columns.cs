namespace UIMarkup.Protocol;

public sealed class Columns : List<Column>
{
	public IElement Owner { get; }

	private Columns() { }

	public Columns(IElement owner)
	{
		Owner = owner;
	}


	public Columns(IElement owner, IEnumerable<Column> collection)
		: base(collection)
	{
		Owner = owner;
	}

	public Columns(IElement owner, int capacity)
		: base(capacity)
	{
		Owner = owner;
	}
}
