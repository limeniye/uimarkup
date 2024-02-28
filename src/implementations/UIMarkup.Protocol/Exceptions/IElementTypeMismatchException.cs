namespace UIMarkup.Protocol;

public class IElementTypeMismatchException : Exception
{
	public IElementTypeMismatchException(Type exceptionType)
		: base($"The {exceptionType} does not match the {nameof(IElement)} type")
	{
	}

	public static void Verify(Type verifyingType)
	{
		if (!verifyingType.IsEquivalentTo(typeof(IElement)))
		{
			throw new IElementTypeMismatchException(verifyingType);
		}
	}
}
