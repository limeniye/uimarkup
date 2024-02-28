using System.Collections.ObjectModel;
using System.Extended.Reflection;

namespace UIMarkup.Protocol;

internal interface IProtocolControl
{
	static ReadOnlyCollection<PropertyInfo> GetProtocolProperties => throw new NotImplementedException();
	ReadOnlyCollection<PropertyValue> GetProtocolPropertyValues();
}
