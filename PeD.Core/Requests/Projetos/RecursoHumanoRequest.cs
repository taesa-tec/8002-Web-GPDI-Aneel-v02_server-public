using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos
{
    public class RecursoHumanoRequest : BaseEntity
    {
        public string NomeCompleto { get; set; }
        public string Titulacao { get; set; }
        public string Funcao { get; set; }
        public string Nacionalidade { get; set; }
        public int EmpresaId { get; set; }

        //Cpf ou Passaport
        public string Documento { get; set; }

        public decimal ValorHora { get; set; }
        public string UrlCurriculo { get; set; }
    }

    public class RecursoHumanoRequestValidator : AbstractValidator<RecursoHumanoRequest>
    {
        public RecursoHumanoRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.NomeCompleto).NotEmpty();
            RuleFor(r => r.Titulacao).NotEmpty();
            RuleFor(r => r.Funcao).NotEmpty();
            RuleFor(r => r.Nacionalidade).NotEmpty();
            RuleFor(r => r.EmpresaId).GreaterThan(0);
            RuleFor(r => r.Documento).NotEmpty();
            RuleFor(r => r.ValorHora).NotEmpty();
        }
    }
}