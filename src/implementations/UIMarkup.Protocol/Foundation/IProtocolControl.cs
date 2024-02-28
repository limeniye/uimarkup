using System.Collections.ObjectModel;
using System.Extended.Reflection;

namespace UIMarkup.Protocol.Foundation;

internal interface IProtocolControl
{
	static ReadOnlyCollection<PropertyInfo> GetProtocolProperties => throw new NotImplementedException();
	ReadOnlyCollection<PropertyValue> GetProtocolPropertyValues();
}
