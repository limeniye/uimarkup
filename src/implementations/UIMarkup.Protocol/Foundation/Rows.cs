namespace UIMarkup.Protocol;

public class Rows : List<Row>
{
	public IElement Owner { get; }

	private Rows() { }

	public Rows(IElement owner)
	{
		Owner = owner;
	}


	public Rows(IElement owner, IEnumerable<Row> collection)
		: base(collection)
	{
		Owner = owner;
	}

	public Rows(IElement owner, int capacity)
		: base(capacity)
	{
		Owner = owner;
	}
}
