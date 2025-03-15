class FolderMailerService
{

    private readonly FolderScannerComponent folderScanner;
    private readonly EmailSenderComponent emailSender;

    public FolderMailerService(FolderScannerComponent folderScannerComponent, EmailSenderComponent emailSenderComponent)
    {
        folderScanner = folderScannerComponent;
        emailSender = emailSenderComponent;
    }


    public void StartMonitoring()
    {
        while (true)
        {
            var files = folderScanner.GetNewFiles();
            foreach (var file in files)
            {
                TryToProcessingFile(file);
            }

            // Ожидание перед следующим опросом
            Thread.Sleep(folderScanner.GetPollingInterval());
        }
    }

    private void TryToProcessingFile(String file)
    {
        try
        {
            ProcessingFile(file);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке файла {file}: {ex.Message}");
        }
    }

    private void ProcessingFile(String file)
    {
        emailSender.SendEmail(file);
        folderScanner.MoveToSent(file);
        Console.WriteLine($"Файл {file} успешно отправлен и перемещен.");
    }
}

