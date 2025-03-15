namespace FolderMailer.Model
{
    public record SmtpSettings(
        string Server,
        int Port,
        string Username,
        string Password,
        string From,
        string SubjectTemplate,
        string BodyTemplate);
}
