using System;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Core.Equipe;
using APIGestor.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace APIGestor.Business.Sistema
{
    public class MailerService
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration configuration;
        private readonly string urlSite;
        private readonly string contactEmail;

        private readonly IConfigurationSection SendGrid;
        public MailerService(
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
            this.SendGrid = this.configuration.GetSection("SendGrid");
            this.urlSite = this.configuration.GetValue<string>("Url");
            this.contactEmail = this.configuration.GetValue<string>("ContactEmail");
        }

        private string createEmailBody(Dictionary<string, string> Dictionary, string file)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates/" + file + ".html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{URL_SITE}}", configuration.GetValue<string>("Url"));
            body = body.Replace("{{CONTACT_EMAIL}}", configuration.GetValue<string>("ContactEmail"));
            foreach (var item in Dictionary)
            {
                body = body.Replace(String.Format("{{{{{0}}}}}", item.Key), item.Value);

            }
            return body;
        }

        public Task<Response> SendMail(ApplicationUser user, string subject, string file, Dictionary<string, string> Dictionary)
        {
            var htmlContent = this.createEmailBody(Dictionary, file);
            return this.SendMail(user, subject, htmlContent);
        }

        public Task<Response> SendMail(ApplicationUser user, string subject, string content)
        {
            var client = new SendGridClient(SendGrid.GetValue<string>("ApiKey"));
            var from = new EmailAddress("noreply@taesa.com.br", "Taesa");
            var to = new EmailAddress(user.Email, user.NomeCompleto);
            var plainTextContent = "";
            var htmlContent = content;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
            return response;
        }
        public Task<Response> SendMailBase(ApplicationUser user, string subject, string Content, (string text, string url) action = default((string text, string url)))
        {
            string Header = subject, Action = "", Body;
            if (!String.IsNullOrEmpty(action.text) && !String.IsNullOrEmpty(action.url))
            {
                var url = (action.url.StartsWith('/') ? configuration.GetValue<string>("Url") : "") + action.url;
                Action = String.Format("<a href=\"{0}\" style=\"display: inline-block; background: #042769; font-weight: bold; color:#FFF; border-radius: 4px; padding: 8px 16px; text-decoration: none; text-transform: uppercase;\">{1}</a>",
                url, action.text);
            }

            Body = createEmailBody(new Dictionary<string, string>(){
                {"HEADER",Header},
                {"CONTENT", Content},
                {"ACTION", Action}
            }, "mail-base");

            var userTeste = new ApplicationUser() { Email = "diego.franca@lojainterativa.com", NomeCompleto = "Diego França" };

            return SendMail(userTeste, subject, Body);
        }
    }
}