using System;
using System.Collections.Generic;
using FluentValidation;

namespace PeD.Core.Requests
{
    public class InstallRequest
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public string AppName { get; set; }
        public string ContactEmail { get; set; }
        public string SpaPath { get; set; }
        public string StoragePath { get; set; }
        public SendGrid SendGrid { get; set; }
        public string SecurityToken { get; set; }
        public Uri Url { get; set; }
        public TokenConfigurations TokenConfigurations { get; set; }
        public Logging Logging { get; set; }
        public AdminUser AdminUser { get; set; }
        public string AllowedHosts { get; set; }
    }

    public class AdminUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ConnectionStrings
    {
        public string BaseGestor { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
    }

    public class SendGrid
    {
        public string ApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }

    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public long Seconds { get; set; }
        public string BaseHash { get; set; }
    }


    public class InstallRequestValidator : AbstractValidator<InstallRequest>
    {
        public InstallRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.ConnectionStrings).NotNull();
            RuleFor(r => r.ConnectionStrings.BaseGestor).NotEmpty();
            RuleFor(r => r.SendGrid.ApiKey).NotEmpty();
            RuleFor(r => r.SendGrid.SenderEmail).NotEmpty();
            RuleFor(r => r.SendGrid.SenderName).NotEmpty();
            RuleFor(r => r.SpaPath).NotEmpty();
            RuleFor(r => r.StoragePath).NotEmpty();
            RuleFor(r => r.Url).NotEmpty();
            RuleFor(r => r.AllowedHosts).NotEmpty();
            RuleFor(r => r.AdminUser).NotNull();
            RuleFor(r => r.AdminUser.Email).NotEmpty();
            RuleFor(r => r.AdminUser.Password).NotEmpty();
            RuleFor(r => r.TokenConfigurations.Audience).NotEmpty();
            RuleFor(r => r.TokenConfigurations.Issuer).NotEmpty();
            RuleFor(r => r.TokenConfigurations.Seconds).GreaterThanOrEqualTo(60);
            RuleFor(r => r.TokenConfigurations.BaseHash).NotEmpty();
        }
    }
}