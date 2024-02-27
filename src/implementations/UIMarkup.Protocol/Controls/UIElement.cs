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
	/// Information about type properties
	/// </summary>
	/// <remarks>
	/// When generating a type that inherits from the UIElement class, the value of this field will be redefined via the new operator.
	/// </remarks>
	public static readonly ReadOnlyCollection<PropertyInfo> ProtocolProperties = new(
	[
		new PropertyInfo(typeof(UIElement), typeof(Guid), nameof(Guid)),
		new PropertyInfo(typeof(UIElement), typeof(string), nameof(Name)),
		new PropertyInfo(typeof(UIElement), typeof(double), nameof(Width)),
		new PropertyInfo(typeof(UIElement), typeof(double), nameof(Height)),
		new PropertyInfo(typeof(UIElement), typeof(double), nameof(MinWidth)),
		new PropertyInfo(typeof(UIElement), typeof(double), nameof(MinHeight)),
		new PropertyInfo(typeof(UIElement), typeof(double), nameof(MaxWidth)),
		new PropertyInfo(typeof(UIElement), typeof(double), nameof(MaxHeight)),
		new PropertyInfo(typeof(UIElement), typeof(HorizontalAlignment), nameof(HorizontalAlignment)),
		new PropertyInfo(typeof(UIElement), typeof(VerticalAlignment), nameof(VerticalAlignment)),
		new PropertyInfo(typeof(UIElement), typeof(Color), nameof(Color)),
		new PropertyInfo(typeof(UIElement), typeof(Style), nameof(Style)),
		new PropertyInfo(typeof(UIElement), typeof(Thickness), nameof(Margin)),
		new PropertyInfo(typeof(UIElement), typeof(double), nameof(Opacity)),
		new PropertyInfo(typeof(UIElement), typeof(bool), nameof(IsVisible)),
		new PropertyInfo(typeof(UIElement), typeof(bool), nameof(IsEnabled))
	]);

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
