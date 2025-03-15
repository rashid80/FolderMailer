using FolderMailer.Model;
using System.IO;

public class FolderScannerComponent
{
    private readonly FolderWatcherSettings settings;

    public FolderScannerComponent(FolderWatcherSettings folderWatcherSettings)
    {
        settings = folderWatcherSettings;
    }

    public string[] GetNewFiles()
    {
        CreateDirectoryIfNeeded(settings.FolderPath);

        return Directory.GetFiles(settings.FolderPath);
    }

    public void MoveToSent(string filePath)
    {
        var sentFolder = Path.Combine(settings.FolderPath, settings.SentFolder);
        CreateDirectoryIfNeeded(sentFolder);

        var fileName = Path.GetFileName(filePath);
        var destFile = Path.Combine(sentFolder, fileName);
        File.Move(filePath, destFile);
    }

    public int GetPollingInterval()
    {
        return settings.PollingInterval;
    }

    private void CreateDirectoryIfNeeded(String directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
