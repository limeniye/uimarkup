using System.Collections.ObjectModel;

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
	/// Contains properties and values ​​of generated types
	/// </summary>
	/// <remarks>
	/// Replaced in derived classes.
	/// </remarks>
	private protected Dictionary<string, object> PropertyValuesMutableProtected = new Dictionary<string, object>()
	{
		{nameof(Guid), Guid},
		{nameof(Name), Name},
		{nameof(Width), Width},
		{nameof(Height), Height},
		{nameof(MinWidth), MinWidth},
		{nameof(MinHeight), MinHeight},
		{nameof(MaxWidth), MaxWidth},
		{nameof(MaxHeight), MaxHeight},
		{nameof(HorizontalAlignment), HorizontalAlignment},
		{nameof(VerticalAlignment), VerticalAlignment},
		{nameof(Color), Color},
		{nameof(Style), Style},
		{nameof(Margin), Margin},
		{nameof(Opacity), Opacity},
		{nameof(IsVisible), IsVisible},
		{nameof(IsEnabled), IsEnabled}
	};

	internal ReadOnlyDictionary<string, object> PropertyValues => new ReadOnlyDictionary<string, object>(PropertyValuesMutableProtected);
}
