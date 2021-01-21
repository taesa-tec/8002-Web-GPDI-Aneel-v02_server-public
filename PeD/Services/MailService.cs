using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PeD.Core.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PeD.Services
{
    public class MailService
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration configuration;
        private readonly string urlSite;
        private readonly string contactEmail;

        private readonly IConfigurationSection SendGrid;

        public MailService(
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
            this.SendGrid = this.configuration.GetSection("SendGrid");
            this.urlSite = this.configuration.GetValue<string>("Url");
            this.contactEmail = this.configuration.GetValue<string>("ContactEmail");
        }

        private string createEmailBody(ApplicationUser user, string file)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates/" + file + ".html")))
            {
                body = reader.ReadToEnd();
            }

            var ResetToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            body = body.Replace("{{URL_SITE}}", this.urlSite);
            body = body.Replace("{{USER_EMAIL}}", user.Email);
            body = body.Replace("{{USER_TOKEN}}", ResetToken);
            body = body.Replace("{{CONTACT_EMAIL}}", this.configuration.GetValue<string>("ContactEmail"));
            body = body.Replace("{{APP_NAME}}", this.configuration.GetValue<string>("AppName"));
            return body;
        }

        //        private string apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
        public bool SendMail(ApplicationUser user, string subject, string file)
        {
            try
            {
                var client = new SendGridClient(SendGrid.GetValue<string>("ApiKey"));
                var from = new EmailAddress(SendGrid.GetValue<string>("SenderEmail"),
                    SendGrid.GetValue<string>("SenderName"));
                var to = new EmailAddress(user.Email, user.NomeCompleto);
                var plainTextContent = "";
                var htmlContent = this.createEmailBody(user, file);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}