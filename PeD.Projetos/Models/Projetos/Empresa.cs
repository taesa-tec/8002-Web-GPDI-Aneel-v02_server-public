using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Attributes;
using PeD.Core.Models.Catalogs;
using CatalogEmpresa = PeD.Projetos.Models.Catalogs.CatalogEmpresa;
using Estado = PeD.Projetos.Models.Catalogs.Estado;

namespace PeD.Projetos.Models.Projetos {
    public class Empresa {
        private int _id;

        [Key]
        public int Id {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        [Logger("Classificação", "ClassificacaoValor")]
        public EmpresaClassificacao Classificacao { get; set; }
        [NotMapped]
        public string ClassificacaoValor { get => Enum.GetName(typeof(EmpresaClassificacao), Classificacao); }
        [Logger("Empresa", "CatalogEmpresa.Nome")]
        public int? CatalogEmpresaId { get; set; }
        public CatalogEmpresa CatalogEmpresa { get; set; }
        [Logger]
        public string Cnpj { get; set; }
        [Logger("Estado", "Estado.Nome")]
        public int? CatalogEstadoId { get; set; }
        [ForeignKey("CatalogEstadoId")]
        public Estado Estado { get; set; }
        [Logger("Razão Social")]
        public string RazaoSocial { get; set; }

        [NotMapped]
        public string NomeEmpresa {
            get {
                if(this.CatalogEmpresa != null) {
                    return this.CatalogEmpresa.Nome;
                }
                else if(this.RazaoSocial != null) {
                    return this.RazaoSocial;
                }
                return "";
            }
        }

    }
    public enum EmpresaClassificacao {
        Proponente, Energia, Executora, Parceira
    }
}