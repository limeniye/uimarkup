using System.Runtime.CompilerServices;

namespace UIMarkup.Protocol.SourceGenerators.Tests;

public static class ModuleInitializer
{
	[ModuleInitializer]
	public static void Init() => VerifySourceGenerators.Initialize();
}
