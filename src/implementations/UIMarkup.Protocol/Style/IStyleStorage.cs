namespace UIMarkup.Protocol;

public interface IStyleStorage : IDisposable
{
	public Task<Style> Get(string styleKey);
}
