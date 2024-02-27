using System.Collections.ObjectModel;
using System.Extended.Reflection;

namespace UIMarkup.Protocol;

public record Control(
	Guid Guid,
	string Name = null,
	double Width = 0, double Height = 0,
	double MinWidth = 0, double MinHeight = 0,
	double MaxWidth = 0, double MaxHeight = 0,
	HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Stretch,
	VerticalAlignment VerticalAlignment = VerticalAlignment.Stretch,
	Color Color = default,
	Style Style = null,
	Template Template = null,
	Thickness Margin = default,
	Thickness CornerRadius = default,
	Thickness Stroke = default,
	Color StrokeColor = default,
	double Opacity = 1.0d,
	bool IsVisible = true,
	bool IsEnabled = true)
		//
		// Inheritance
		//
		: UIElement(Guid, Name, Width, Height, MinWidth, MinHeight,
			MaxWidth, MaxHeight, HorizontalAlignment, VerticalAlignment,
			Color, Style, Margin, Opacity, IsVisible, IsEnabled)
{
	/// <inheritdoc cref="UIElement.ProtocolProperties" /> 
	internal new static readonly ReadOnlyCollection<PropertyInfo> ProtocolProperties = new(
	[
		new PropertyInfo(typeof(Control), typeof(Guid), nameof(Guid)),
		new PropertyInfo(typeof(Control), typeof(string), nameof(Name)),
		new PropertyInfo(typeof(Control), typeof(double), nameof(Width)),
		new PropertyInfo(typeof(Control), typeof(double), nameof(Height)),
		new PropertyInfo(typeof(Control), typeof(double), nameof(MinWidth)),
		new PropertyInfo(typeof(Control), typeof(double), nameof(MinHeight)),
		new PropertyInfo(typeof(Control), typeof(double), nameof(MaxWidth)),
		new PropertyInfo(typeof(Control), typeof(double), nameof(MaxHeight)),
		new PropertyInfo(typeof(Control), typeof(HorizontalAlignment), nameof(HorizontalAlignment)),
		new PropertyInfo(typeof(Control), typeof(VerticalAlignment), nameof(VerticalAlignment)),
		new PropertyInfo(typeof(Control), typeof(Color), nameof(Color)),
		new PropertyInfo(typeof(Control), typeof(Style), nameof(Style)),
		new PropertyInfo(typeof(Control), typeof(Thickness), nameof(Margin)),
		new PropertyInfo(typeof(Control), typeof(double), nameof(Opacity)),
		new PropertyInfo(typeof(Control), typeof(bool), nameof(IsVisible)),
		new PropertyInfo(typeof(Control), typeof(bool), nameof(IsEnabled)),

		new PropertyInfo(typeof(Control), typeof(Thickness), nameof(CornerRadius)),
		new PropertyInfo(typeof(Control), typeof(Thickness), nameof(Stroke)),
		new PropertyInfo(typeof(Control), typeof(Color), nameof(StrokeColor))
	]);

	/// <inheritdoc cref="UIElement.ProtocolPropertyValues" /> 
	internal new ReadOnlyCollection<PropertyValue> ProtocolPropertyValues => new(
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
		new PropertyValue(this, typeof(bool), IsEnabled),

		new PropertyValue(this, typeof(Thickness), CornerRadius),
		new PropertyValue(this, typeof(Thickness), Stroke),
		new PropertyValue(this, typeof(Color), StrokeColor)
	]);
}
