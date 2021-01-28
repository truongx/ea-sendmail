using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EA.SendMail.WebAPI
{
    public static class SendMail
    {
        [FunctionName("SendMail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "mail")] SendMailRequest request,
            ILogger log)
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
                var client = new SendGridClient(apiKey);
                SendGridMessage msg;

                if (request.Sender == null || request.Recipients == null || request.Recipients.Count == 0)
                    return new BadRequestResult();

                if (!string.IsNullOrEmpty(request.TemplateId))
                {
                    msg = MailHelper.CreateSingleTemplateEmailToMultipleRecipients(request.Sender, request.Recipients, request.TemplateId, request.TemplateData);
                }
                else
                {
                    msg = MailHelper.CreateSingleEmailToMultipleRecipients(request.Sender, request.Recipients, request.Subject, request.PlainTextContent, request.HtmlContent);
                }

                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode.Equals(HttpStatusCode.Accepted))
                    return new OkResult();
                else
                    throw new Exception($"Sending Email failed with status {response.StatusCode}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}