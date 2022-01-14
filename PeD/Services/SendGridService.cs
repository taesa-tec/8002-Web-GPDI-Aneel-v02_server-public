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
                Logger.LogError("Sendgrid não configurado!");
            }
        }

        public async Task<bool> Send(string to, string subject, string content, string title = null,
            string actionLabel = null, string actionUrl = null)
        {
            return await Send(new[] { to }, subject, content, title, actionLabel, actionUrl);
        }

        public async Task<bool> Send(string[] tos, string subject, string content, string title = null,
            string actionLabel = null, string actionUrl = null)
        {
            title ??= subject;
            return await Send(tos, subject, "Email/SimpleMail",
                new SimpleMail()
                    { Titulo = title, Conteudo = content, ActionLabel = actionLabel, ActionUrl = actionUrl });
        }

        public async Task<bool> Send<T>(string[] tos, string subject, string viewName, T model) where T : class
        {
            try
            {
                if (Client == null) { throw new NullReferenceException(); }

                var viewContent = await ViewRender.RenderToStringAsync(viewName, model);
                var message = MailHelper.CreateSingleEmailToMultipleRecipients(From,
                    tos.Select(to => new EmailAddress(to)).ToList(),
                    subject, "", viewContent);

                foreach (var bcc in EmailConfig.Bcc)
                {
                    if (!tos.Contains(bcc))
                        message.AddBcc(bcc);
                }

                await Client.SendEmailAsync(message);
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError("Erro no disparo de email: {Error}.", e.Message);
                Logger.LogError("StackError: {Error}", e.StackTrace);
                return false;
            }
        }

        public async Task<bool> Send<T>(string to, string subject, string viewName, T model) where T : class
        {
            return await Send(new[] { to }, subject, viewName, model);
        }
    }
}