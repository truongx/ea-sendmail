using SendGrid.Helpers.Mail;
using System.Collections.Generic;

public class SendMailRequest
{
    public string HtmlContent { get; set; }
    public string PlainTextContent { get; set; }
    public List<EmailAddress> Recipients { get; set; }
    public EmailAddress Sender { get; set; }
    public string Subject { get; set; }
    public Dictionary<string, string> TemplateData { get; set; }
    public string TemplateId { get; set; }
}