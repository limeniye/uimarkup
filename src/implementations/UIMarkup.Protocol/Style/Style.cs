using System.Collections.ObjectModel;
using System.Extended;

namespace UIMarkup.Protocol;

public class Style
{
	private readonly Dictionary<string, object> _propertyValues;

	/// <summary>
	/// Describes the default properties
	/// </summary>
	/// <param name="targetType">A target type that inherits from UIElement</param>
	/// <exception cref="UIElementTypeMismatchException">In case the type does not inherit from UIElement.</exception>
	public Style(Type targetType, string name = null)
	{
		Name = name ?? string.Empty;
		TargetType = targetType;
		throw new NotImplementedException();
	}

	public Style(Style basedOn, string name = null)
	{
		Name = name ?? string.Empty;
		TargetType = basedOn.TargetType;
		BasedOn = basedOn;
		_propertyValues = basedOn.PropertyValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
	}

	/// <summary>
	/// Creating the first BaseOn type
	/// <example>
	/// <code>
	/// new Style(UIElement.ProtocolProperties.ToDictionary(kvp => kvp.Name, someValue));
	/// </code>
	/// </example>
	/// </summary>
	/// <param name="propertyValues">
	/// A parameter must have only the set of properties that TargetElement has
	/// </param>
	internal Style(Type targetType, Dictionary<string, object> propertyValues, string name = null)
	{
		Name = name ?? string.Empty;
		TargetType = targetType;
		_propertyValues = propertyValues;
	}

	public Type TargetType { get; }
	public Style BasedOn { get; }
	public string Name { get; }
	public ReadOnlyDictionary<string, object> PropertyValues => new ReadOnlyDictionary<string, object>(_propertyValues);
	public EventHandler<NotifyDictionaryChangedEventArgs<string, object>> PropertyValuesChanged;

	public void ChangePropertyValue(string propertyName, object newValue)
	{
		_propertyValues.ChangeAndRiseEvent(this, PropertyValuesChanged, propertyName, newValue);
	}

	/// <summary>
	/// If BaseOn was not specified, then the property will receive the default value.
	/// If BaseOn is specified, then the property will be taken from the parent style.
	/// </summary>
	public void ResetPropertyValue(string propertyName)
	{
		_propertyValues.ChangeAndRiseEvent(this, PropertyValuesChanged, propertyName, null);
	}

	public void AddAttachedProperty(string propertyName, object value)
	{
		_propertyValues.AddAndRiseEvent(this, PropertyValuesChanged, propertyName, value);
	}

	public void ChangeAttachedProperty(string propertyName, object value)
	{
		_propertyValues.AddAndRiseEvent(this, PropertyValuesChanged, propertyName, value);
	}

	public void RemoveAttachedProperty(string propertyName)
	{
		_propertyValues.RemoveAndRiseEvent(this, PropertyValuesChanged, propertyName);
	}
}
