namespace System.Extended;

public class PropertyValueChangedEventArgs : System.EventArgs
{
	public PropertyValueChangedEventArgs(string propertyName, object value)
	{
		PropertyName = propertyName;
		Value = value;
	}

	public string PropertyName { get; }

	public object Value { get; }
}
