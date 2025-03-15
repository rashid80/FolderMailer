using FolderMailer.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class ConfigurationLoader
{
    private const String CONFIGURATION_FILE_NAME = "appsettings.json";

    private readonly IConfiguration configuration;

    public ConfigurationLoader()
    {
        configuration = LoadConfiguration();
    }

    // Метод для получения настроек FolderWatcher
    public FolderWatcherSettings GetFolderWatcherSettings()
    {
        return configuration.GetSection("FolderWatcher").Get<FolderWatcherSettings>();
    }

    // Метод для получения настроек SmtpSettings
    public SmtpSettings GetSmtpSettings()
    {
        return configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
    }

    private IConfiguration LoadConfiguration()
    {
        var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), CONFIGURATION_FILE_NAME);

        // Если файла нет, создаем его с дефолтными настройками
        if (!File.Exists(configFilePath))
        {
            CreateDefaultConfigurationFile(configFilePath);
        }

        // Загрузка конфигурации
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(CONFIGURATION_FILE_NAME, optional: true, reloadOnChange: true)
            .Build();
    }

    private void CreateDefaultConfigurationFile(String configFilePath)
    {
        var defaultConfig = new
        {
            FolderWatcher = new
            {
                FolderPath = "C:\\watch",
                PollingInterval = 5000,
                SentFolder = "sent"
            },
            SmtpSettings = new
            {
                Server = "smtp.example.com",
                Port = 587,
                Username = "user@example.com",
                Password = "password",
                From = "user@example.com",
                SubjectTemplate = "New file: {0}",
                BodyTemplate = "Please find attached the file: {0}"
            }
        };

        var configurationJson = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(configFilePath, configurationJson);
        Console.WriteLine($"Создан файл {configFilePath} конфигурации с настройками по умолчанию: {configurationJson}");
    }
}
