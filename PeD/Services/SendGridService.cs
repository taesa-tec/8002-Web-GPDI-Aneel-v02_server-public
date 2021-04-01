using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PeD.Core;
using PeD.Views.Email;
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
        protected ILogger<SendGridService> Logger;

        public SendGridService(IViewRenderService viewRender, EmailConfig emailConfig, ILogger<SendGridService> logger)
        {
            EmailConfig = emailConfig;
            Logger = logger;
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

        public async Task Send(string to, string subject, string content, string title = null,
            string actionLabel = null, string actionUrl = null)
        {
            await Send(new List<string> {to}, subject, content, title, actionLabel, actionUrl);
        }

        public async Task Send(IEnumerable<string> tos, string subject, string content, string title = null,
            string actionLabel = null, string actionUrl = null)
        {
            try
            {
                if (Client == null)
                {
                    throw new NullReferenceException();
                }

                title ??= subject;
                await Send(tos, subject, "Email/SimpleMail",
                    new SimpleMail()
                        {Titulo = title, Conteudo = content, ActionLabel = actionLabel, ActionUrl = actionUrl});
            }
            catch (Exception e)
            {
                Logger.LogError("Erro no disparo de email: {Message}", e.Message);
                // @todo Log Send email
            }
        }

        public async Task Send<T>(IEnumerable<string> tos, string subject, string viewName, T model) where T : class
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
                if (!tos.Contains("diego.franca@lojainterativa.com"))
                    message.AddBcc("diego.franca@lojainterativa.com", "Diego");
                await Client.SendEmailAsync(message);
            }
            catch (Exception e)
            {
                // @todo Log Send email
            }
        }

        public async Task Send<T>(string to, string subject, string viewName, T model) where T : class
        {
            await Send(new List<string>() {to}, subject, viewName, model);
        }
    }
}