using System;
using System.Collections.Generic;
using FluentValidation;

namespace PeD.Core.Requests
{
    public class InstallRequest
    {
        public SendGrid SendGrid { get; set; }
        public AdminUser AdminUser { get; set; }
    }

    public class AdminUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SendGrid
    {
        public string ApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }

    public class InstallRequestValidator : AbstractValidator<InstallRequest>
    {
        public InstallRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.SendGrid.ApiKey).NotEmpty();
            RuleFor(r => r.SendGrid.SenderEmail).NotEmpty();
            RuleFor(r => r.SendGrid.SenderName).NotEmpty();
            RuleFor(r => r.AdminUser).NotNull();
            RuleFor(r => r.AdminUser.Email).NotEmpty();
            RuleFor(r => r.AdminUser.Password).NotEmpty();
        }
    }
}