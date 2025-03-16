namespace FolderMailer.Model
{
    public record SmtpSettings(
        string Server,
        int Port,
        string Username,
        string Password,
        string FromEmail,
        string FromName,
        string ToEmail,
        string ToName,
        string SubjectTemplate,
        string BodyTemplate);
}
