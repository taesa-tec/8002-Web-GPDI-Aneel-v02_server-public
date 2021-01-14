using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeD.Services.Sistema;
using Microsoft.Extensions.Configuration;
using PeD.Core;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PeD.Services
{
    public class SendGridService
    {
        protected SendGridClient Client;
        protected EmailAddress From;
        protected EmailConfig EmailConfig;
        protected IViewRenderService ViewRender;

        public SendGridService(IViewRenderService viewRender, EmailConfig emailConfig)
        {
            EmailConfig = emailConfig;
            if (!string.IsNullOrEmpty(EmailConfig.ApiKey))
            {
                Client = new SendGridClient(EmailConfig.ApiKey);
                From = new EmailAddress(EmailConfig.SenderEmail, EmailConfig.SenderName);
                ViewRender = viewRender;
            }
            else
            {
                // @todo Log erro
            }
        }

        public async Task Send(string to, string subject, string content)
        {
            await Send(new List<string> {to}, subject, content);
        }

        public async Task Send(IEnumerable<string> tos, string subject, string content)
        {
            try
            {
                if (Client == null)
                {
                    throw new NullReferenceException();
                }

                var viewContent = await ViewRender.RenderToStringAsync("Email/Simple", content);

                var message = MailHelper.CreateSingleEmailToMultipleRecipients(From,
                    tos.Select(to => new EmailAddress(to)).ToList(),
                    subject, content, viewContent);
                message.AddBcc("diego.franca@lojainterativa.com", "Diego");
                await Client.SendEmailAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // @todo Log Send email
            }
        }

        public async Task Send(IEnumerable<string> tos, string subject, string viewName, object model)
        {
            try
            {
                if (Client == null)
                {
                    throw new NullReferenceException();
                }

                var viewContent = await ViewRender.RenderToStringAsync(viewName, model);
                //var message = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, "", viewContent);
                var message = MailHelper.CreateSingleEmailToMultipleRecipients(From,
                    tos.Select(to => new EmailAddress(to)).ToList(),
                    subject, "", viewContent);
                message.AddBcc("diego.franca@lojainterativa.com", "Diego");
                await Client.SendEmailAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // @todo Log Send email
            }
        }

        public async Task Send(string to, string subject, string viewName, object model)
        {
            await Send(new List<string>() {to}, subject, viewName, model);
        }
    }
}