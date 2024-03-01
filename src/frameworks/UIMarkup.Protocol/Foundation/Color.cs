namespace UIMarkup.Protocol;

public struct Color
{
	private string _hex;

	public Color()
	{
		this = Transparent;
	}

	public Color(string hex)
	{
		_hex = hex;
	}

	public readonly static Color Transparent = new Color("#00FFFFFF");

	public string Hex => _hex;
}
