using System.Text.Json.Serialization;

namespace FolderMailer.Model
{
    public record FolderWatcherSettings(
        string FolderPath,
        int PollingInterval,
        string SentFolder,
        string FilePatterns
        )
    {
        [JsonIgnore]
        public string[] SearchPatterns { get; } = FilePatterns
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(pattern => pattern.Trim())
            .ToArray();
    };
}
