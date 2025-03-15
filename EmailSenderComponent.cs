using FolderMailer.Model;

public class EmailSenderComponent
{
    private readonly SmtpSettings settings;

    public EmailSenderComponent(SmtpSettings smtpSettings)
    {
        settings = smtpSettings;
    }

    public void SendEmail(string filePath)
    {
        //var message = new MimeMessage();
        //message.From.Add(new MailboxAddress("FolderWatcher", _settings.From));
        //message.To.Add(new MailboxAddress("Recipient", _settings.From));
        //message.Subject = string.Format(_settings.SubjectTemplate, Path.GetFileName(filePath));

        //var body = new TextPart("plain")
        //{
        //    Text = string.Format(_settings.BodyTemplate, Path.GetFileName(filePath))
        //};

        //var attachment = new MimePart("application", "octet-stream")
        //{
        //    Content = new MimeContent(File.OpenRead(filePath)),
        //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
        //    ContentTransferEncoding = ContentEncoding.Base64,
        //    FileName = Path.GetFileName(filePath)
        //};

        //var multipart = new Multipart("mixed");
        //multipart.Add(body);
        //multipart.Add(attachment);

        //message.Body = multipart;

        //using (var client = new SmtpClient())
        //{
        //    client.Connect(_settings.Server, _settings.Port, SecureSocketOptions.StartTls);
        //    client.Authenticate(_settings.Username, _settings.Password);
        //    client.Send(message);
        //    client.Disconnect(true);
        //}
    }
}