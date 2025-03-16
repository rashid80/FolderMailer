using FolderMailer;
using FolderMailer.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;
using System.Text.Json;

public class ConfigurationLoader
{
    private const String CONFIGURATION_FILE_NAME = "appsettings.json";

    private IConfiguration? configuration;

    public FolderWatcherSettings FolderWatcherSettings { get; private set; }
    public SmtpSettings SmtpSettings { get; private set; }

    public bool IsConfigFileCreated { get; private set; } = false;

    public ConfigurationLoader()
    {
        LoadConfiguration();
        ValidateConfiguration();
    }

    private void LoadConfiguration()
    {
        var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), CONFIGURATION_FILE_NAME);

        // Если файла нет, создаем его с дефолтными настройками
        if (!File.Exists(configFilePath))
        {
            CreateDefaultConfigurationFile(configFilePath);
            IsConfigFileCreated = true;
            return;
        }

        // Загрузка конфигурации
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(CONFIGURATION_FILE_NAME, optional: true, reloadOnChange: true)
            .Build();

        FolderWatcherSettings = configuration.GetSection("FolderWatcher").Get<FolderWatcherSettings>()!;
        SmtpSettings = configuration.GetSection("Smtp").Get<SmtpSettings>()!;

        ValidateConfiguration();
    }

    private void ValidateConfiguration()
    {
        if (FolderWatcherSettings!.SearchPatterns.Length == 0)
        {
            throw new ArgumentException("Пустая маска файлов в блоке FolderWatcherSettings");
        }
    }

    private void CreateDefaultConfigurationFile(String configFilePath)
    {

        FolderWatcherSettings = new FolderWatcherSettings(
            FolderPath: "C:\\watch",
            PollingInterval: 60*1000, // 1 минута
            SentFolder: "sent",
            FilePatterns: "*.xls,*.xlsx"
        );

        SmtpSettings = new SmtpSettings(
            Server: "smtp.example.com",
            Port: 587,
            Username: "user@example.com",
            Password: "password",
            FromEmail: "from@example.com",
            FromName: "Имя отправителя",
            ToEmail: "to@example.com",
            ToName: "Имя получателя",
            SubjectTemplate: "Заказ {0}",
            BodyTemplate: "Сформирован новый заказ: {0}"
        );

        // Создаем объект конфигурации
        var defaultConfig = new
        {
            FolderWatcher = FolderWatcherSettings,
            Smtp = SmtpSettings
        };

        // Настройки сериализации
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Разрешаем русские символы
            WriteIndented = true // Читаемый JSON с отступами
        };

        // Сериализуем в JSON
        var configurationJson = JsonSerializer.Serialize(defaultConfig, options);

        File.WriteAllText(configFilePath, configurationJson);
        ConsoleLogger.Debug($"Создан файл {configFilePath} с настройками по умолчанию: {Environment.NewLine}{configurationJson}");
    }
}
