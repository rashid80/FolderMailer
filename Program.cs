using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        ConsoleLogger.Debug("FolderMailer запущен.");

        // Загрузка конфигурации
        var configurationLoader = new ConfigurationLoader();

        // Если файл конфигурации был создан, завершаем работу
        if (configurationLoader.IsConfigFileCreated)
        {
            ConsoleLogger.Debug("Пожалуйста, заполните созданный файл с настройками и перезапустите приложение.");
            ConsoleLogger.Debug("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
            return;
        }

        // Настройка DI
        var services = new ServiceCollection();
        services.AddSingleton(configurationLoader.FolderWatcherSettings);
        services.AddSingleton(configurationLoader.SmtpSettings);

        services.AddSingleton<FolderScannerComponent>();
        services.AddSingleton<EmailSenderComponent>();
        services.AddSingleton<FolderMailerService>();
        var serviceProvider = services.BuildServiceProvider();

        // Получаем экземпляр FolderMailerService и запускаем мониторинг
        var folderMailer = serviceProvider.GetService<FolderMailerService>()!;
        folderMailer.StartMonitoring();

        ConsoleLogger.Debug("FolderMailer завершил работу.");
    }
}