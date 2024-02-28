using System.Collections.ObjectModel;
using System.Extended.Reflection;

namespace UIMarkup.Protocol;

public record UIElement(
	Guid Guid,
	string Name = null,
	double Width = 0, double Height = 0,
	double MinWidth = 0, double MinHeight = 0,
	double MaxWidth = 0, double MaxHeight = 0,
	HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Stretch,
	VerticalAlignment VerticalAlignment = VerticalAlignment.Stretch,
	Color Color = default,
	Style Style = null,
	Thickness Margin = default,
	double Opacity = 1.0d,
	bool IsVisible = true,
	bool IsEnabled = true)
{
	/// <summary>
	/// Contains properties and values ​​of generated UIElement types.
	/// Replaced in derived classes.
	/// </summary>
	/// <remarks>
	/// When generating a type that inherits from the UIElement class, the value of this field will be redefined via the new operator.
	/// </remarks>
	internal ReadOnlyCollection<PropertyValue> ProtocolPropertyValues => new(
	[
		new PropertyValue(this, typeof(Guid), Guid),
		new PropertyValue(this, typeof(string), Name),
		new PropertyValue(this, typeof(double), Width),
		new PropertyValue(this, typeof(double), Height),
		new PropertyValue(this, typeof(double), MinWidth),
		new PropertyValue(this, typeof(double), MinHeight),
		new PropertyValue(this, typeof(double), MaxWidth),
		new PropertyValue(this, typeof(double), MaxHeight),
		new PropertyValue(this, typeof(HorizontalAlignment), HorizontalAlignment),
		new PropertyValue(this, typeof(VerticalAlignment), VerticalAlignment),
		new PropertyValue(this, typeof(Color), Color),
		new PropertyValue(this, typeof(Style), Style),
		new PropertyValue(this, typeof(Thickness), Margin),
		new PropertyValue(this, typeof(double), Opacity),
		new PropertyValue(this, typeof(bool), IsVisible),
		new PropertyValue(this, typeof(bool), IsEnabled)
	]);
}
