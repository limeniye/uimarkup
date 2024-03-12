namespace UIMarkup.Protocol.SourceGenerators;

public class JsonFileMonitor
{
	private readonly string jsonFilePath;
	private FileSystemWatcher watcher;
	private readonly ControlGenerator generator;

	public JsonFileMonitor(string filePath, ControlGenerator sourceGenerator)
	{
		jsonFilePath = filePath;
		generator = sourceGenerator;

		InitializeWatcher();
	}

	private void InitializeWatcher()
	{
		watcher = new FileSystemWatcher(Path.GetDirectoryName(jsonFilePath))
		{
			Filter = Path.GetFileName(jsonFilePath),
			NotifyFilter = NotifyFilters.LastWrite
		};

		watcher.Changed += OnJsonFileChanged;
		watcher.EnableRaisingEvents = true;
	}

	private void OnJsonFileChanged(object sender, FileSystemEventArgs e)
	{
		if (e.ChangeType == WatcherChangeTypes.Changed)
		{
			generator.Execute();
		}
	}

	public void StopMonitoring()
	{
		watcher.EnableRaisingEvents = false;
		watcher.Changed -= OnJsonFileChanged;
		watcher.Dispose();
	}
}
