using Microsoft.Extensions.DependencyInjection;
using FolderMailer.Model;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("FolderMailer запущен.");

        // Загрузка конфигурации
        var configuration = new ConfigurationLoader();

        // Настройка DI
        var services = new ServiceCollection();
        services.AddSingleton(configuration.GetFolderWatcherSettings());
        services.AddSingleton(configuration.GetSmtpSettings());

        services.AddSingleton<FolderScannerComponent>();
        services.AddSingleton<EmailSenderComponent>();
        services.AddSingleton<FolderMailerService>();
        var serviceProvider = services.BuildServiceProvider();

        // Получаем экземпляр FolderMailerService и запускаем мониторинг
        var folderMailer = serviceProvider.GetService<FolderMailerService>();
        folderMailer.StartMonitoring();

        Console.WriteLine("FolderMailer завершил работу.");
    }
}