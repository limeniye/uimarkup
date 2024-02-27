namespace UIMarkup.Protocol;

public class UIElementTypeMismatchException : Exception
{
	public UIElementTypeMismatchException(Type exceptionType)
		: base($"The {exceptionType} does not match the {nameof(UIElement)} type")
	{
	}

	public static void Verify(Type verifyingType)
	{
		if (!verifyingType.IsEquivalentTo(typeof(UIElement)))
		{
			throw new UIElementTypeMismatchException(verifyingType);
		}
	}
}
