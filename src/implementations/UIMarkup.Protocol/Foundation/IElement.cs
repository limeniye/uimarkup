namespace UIMarkup.Protocol;

public interface IElement
{
	string Name { get; }
	double Width { get; }
	double Height { get; }
	double MinWidth { get; }
	double MinHeight { get; }
	double MaxWidth { get; }
	double MaxHeight { get; }
	HorizontalAlignment HorizontalAlignment { get; }
	VerticalAlignment VerticalAlignment { get; }
	Color Color { get; }
	Style Style { get; }
	Thickness Margin { get; }
	double Opacity { get; }
	bool IsVisible { get; }
	bool IsEnabled { get; }
}
