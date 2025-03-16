using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using FolderMailer.Model;
using FolderMailer;

public class EmailSenderComponent(SmtpSettings smtpSettings)
{
    private const int sendEmailWaiting = 1000;
    
    private readonly SmtpSettings settings = smtpSettings;


    public bool SendEmailSafety(string filePath)
    {
        try
        {
            SendEmail(filePath);
            ConsoleLogger.Success($"Файл {filePath} успешно отправлен на адрес: {settings.ToEmail}");
            return true;
        }
        catch (Exception ex) {
            ConsoleLogger.Error($"ERR: Не удалось отправить файл {filePath} по причине: {ex.Message}");
            return false;
        }
    }

    private void SendEmail(string filePath)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(settings.FromName, settings.FromEmail));
        message.To.Add(new MailboxAddress(settings.ToName, settings.ToEmail));
        message.Subject = string.Format(settings.SubjectTemplate, Path.GetFileName(filePath));

        var body = new TextPart("plain")
        {
            Text = string.Format(settings.BodyTemplate, Path.GetFileName(filePath))
        };


        using var fileStream = File.OpenRead(filePath);
        var attachment = new MimePart("application", "octet-stream")
        {
            Content = new MimeContent(fileStream),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = Path.GetFileName(filePath)
        };

        var multipart = new Multipart("mixed")
        {
            body,
            attachment
        };
        message.Body = multipart;

        using var client = new SmtpClient();
        client.Connect(settings.Server, settings.Port, SecureSocketOptions.StartTls);
        client.Authenticate(settings.Username, settings.Password);
        client.Send(message);
        client.Disconnect(true);

        Thread.Sleep(sendEmailWaiting);
    }
}