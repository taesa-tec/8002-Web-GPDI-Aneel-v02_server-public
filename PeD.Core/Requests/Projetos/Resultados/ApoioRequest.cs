using System.ComponentModel.DataAnnotations;
using FluentValidation;
using PeD.Core.Utils;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class ApoioRequest : BaseEntity
    {
        public string Tipo { get; set; }

        public string CnpjReceptora { get; set; }
        [MaxLength(100)] public string Laboratorio { get; set; }
        [MaxLength(50)] public string LaboratorioArea { get; set; }
        [MaxLength(300)] public string MateriaisEquipamentos { get; set; }
    }

    public class ApoioRequestValidor : AbstractValidator<ApoioRequest>
    {
        public ApoioRequestValidor()
        {
            RuleFor(r => r.Tipo).NotEmpty();
            RuleFor(r => r.CnpjReceptora).NotEmpty().Custom((s, context) =>
            {
                if (!CpfCnpj.IsCnpj(s))
                    context.AddFailure("CNPJ inválido");
            });
            RuleFor(r => r.Laboratorio).NotEmpty();
            RuleFor(r => r.LaboratorioArea).NotEmpty();
            RuleFor(r => r.MateriaisEquipamentos).NotEmpty();
        }
    }
}