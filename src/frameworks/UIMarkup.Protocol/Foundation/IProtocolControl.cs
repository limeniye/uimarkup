using System.Collections.ObjectModel;
using System.Extended.Reflection;

namespace UIMarkup.Protocol;

internal interface IProtocolControl
{
	ReadOnlyCollection<PropertyValue> GetProtocolPropertyValues();
}
