using FluentValidation;
using PeD.Core.Models.Propostas;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class EmpresaRequest : BaseEntity
    {
        public int? EmpresaRefId { get; set; }
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string Codigo { get; set; }
        public string UF { get; set; }
        public Funcao Funcao { get; set; }
    }

    public class EmpresaRequestValidator : AbstractValidator<EmpresaRequest>
    {
        public EmpresaRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.RazaoSocial).NotEmpty();
            RuleFor(r => r.Funcao).NotNull();
            
            //Cooperada
            RuleFor(r => r.Codigo).NotEmpty().When(r=>r.Funcao == Funcao.Cooperada);
            
            // Executora
            RuleFor(r => r.UF).NotEmpty().When(r=>r.Funcao == Funcao.Executora);
            RuleFor(r => r.CNPJ).NotEmpty().When(r=>r.Funcao == Funcao.Executora);
            
        }
    }
}