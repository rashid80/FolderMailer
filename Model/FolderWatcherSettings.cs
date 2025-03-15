using System;

namespace FolderMailer.Model
{
    public record FolderWatcherSettings(
        string FolderPath,
        int PollingInterval,
        string SentFolder
        );
}
