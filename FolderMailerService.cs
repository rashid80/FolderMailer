using FolderMailer;

class FolderMailerService(FolderScannerComponent folderScannerComponent, EmailSenderComponent emailSenderComponent)
{

    private readonly FolderScannerComponent folderScanner = folderScannerComponent;
    private readonly EmailSenderComponent emailSender = emailSenderComponent;

    public void StartMonitoring()
    {
        while (true)
        {
            var files = folderScanner.GetNewFiles();
            foreach (var file in files)
            {
                ProcessingFile(file);
            }

            // Ожидание перед следующим опросом
            Thread.Sleep(folderScanner.GetPollingInterval());
        }
    }

    private void ProcessingFile(String file)
    {
        if (!emailSender.SendEmailSafety(file))
        {
            return;
        }

        if (!folderScanner.MoveToSentSafety(file))
        {
            return;
        }

        ConsoleLogger.Debug($"Файл {file} успешно отправлен и перемещен.");
    }
}

