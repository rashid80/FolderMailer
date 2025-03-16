using FolderMailer;
using FolderMailer.Model;

public class FolderScannerComponent(FolderWatcherSettings folderWatcherSettings)
{
    private readonly FolderWatcherSettings settings = folderWatcherSettings;

    private readonly string sentFolder = Path.Combine(folderWatcherSettings.FolderPath, folderWatcherSettings.SentFolder);

    public string[] GetNewFiles()
    {
        CreateDirectoryIfNeeded(settings.FolderPath);

        // Ищем файлы по нескольким маскам
        return settings.SearchPatterns
            .SelectMany(pattern => Directory.GetFiles(settings.FolderPath, pattern))
            .Distinct() // Убираем дубликаты, если файл подходит под несколько масок
            .ToArray();
    }

    public bool MoveToSentSafety(string filePath)
    {
        try
        {
            MoveToSent(filePath);
            ConsoleLogger.Success($"Файл {filePath} успешно перемешен в папку {sentFolder}");
            return true;
        }
        catch (Exception ex)
        {
            ConsoleLogger.Error($"ERR: Не удалось переместить файл {filePath} в папку {sentFolder} по причине: {ex.Message}");
            return false;
        }
    }

    private void MoveToSent(string filePath)
    {
        CreateDirectoryIfNeeded(sentFolder);

        var fileName = Path.GetFileName(filePath);
        var destFile = Path.Combine(sentFolder, fileName);

        // Копируем файл с перезаписью
        File.Copy(filePath, destFile, overwrite: true);

        // Удаляем исходный файл
        File.Delete(filePath);
    }

    public int GetPollingInterval()
    {
        return settings.PollingInterval;
    }

    private void CreateDirectoryIfNeeded(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
