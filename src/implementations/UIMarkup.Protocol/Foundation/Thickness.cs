using System.Drawing;

namespace UIMarkup.Protocol;

public struct Thickness
{
	public double Left { get; set; }

	public double Top { get; set; }

	public double Right { get; set; }

	public double Bottom { get; set; }

	public Thickness()
	{
		this = Zero;
	}

	public Thickness(double uniformSize)
		: this(uniformSize, uniformSize, uniformSize, uniformSize)
	{
	}

	public Thickness(double horizontalThickness, double verticalThickness)
		: this(horizontalThickness, verticalThickness, horizontalThickness, verticalThickness)
	{
	}

	public Thickness(double left, double top, double right, double bottom)
		: this()
	{
		Left = left;
		Top = top;
		Right = right;
		Bottom = bottom;
	}

	public static Thickness Zero = new Thickness(0);

	public static implicit operator Thickness(Size size)
	{
		return new Thickness(size.Width, size.Height, size.Width, size.Height);
	}

	public static implicit operator Thickness(double uniformSize)
	{
		return new Thickness(uniformSize);
	}

	public static Thickness operator +(Thickness left, double addend) =>
		new Thickness(left.Left + addend, left.Top + addend, left.Right + addend, left.Bottom + addend);

	public static Thickness operator +(Thickness left, Thickness right) =>
		new Thickness(left.Left + right.Left, left.Top + right.Top, left.Right + right.Right, left.Bottom + right.Bottom);

	public static Thickness operator -(Thickness left, double addend) =>
		left + (-addend);
}
