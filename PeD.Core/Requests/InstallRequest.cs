using System;
using System.Collections.Generic;
using FluentValidation;

namespace PeD.Core.Requests
{
    public class InstallRequest
    {
        public string NomeCompleto { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }


    public class InstallRequestValidator : AbstractValidator<InstallRequest>
    {
        public InstallRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(r => r.NomeCompleto).NotEmpty();
            RuleFor(r => r.Cargo).NotEmpty();
            RuleFor(r => r.Password).NotEmpty();
            RuleFor(r => r.ConfirmPassword).Must((request, s) => s == request.Password);
        }
    }
}