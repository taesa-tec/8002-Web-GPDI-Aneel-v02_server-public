using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Attributes;
using PeD.Core.Models.Catalogs;

namespace PeD.Core.Models.Projetos {
    public enum CategoriaContabil {
        RH, ST, MC, MP, VD, OU
    }

    public class RecursoMaterial {
        [Key]
        public int Id { get; set; }
        public int? ProjetoId { get; set; }
        [Logger]
        public string Nome { get; set; }
        [Logger("Categoria Contábil", "categoria")]
        public CategoriaContabil CategoriaContabil { get; set; }
        [NotMapped]
        public string CategoriaContabilValor { get => Enum.GetName(typeof(CategoriaContabil), CategoriaContabil); }

        [Logger("Valor Unitário")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ValorUnitario { get; set; }
        [Logger("Especificação")]
        public string Especificacao { get; set; }
        [Logger("Categoria Contábil", "categoria")]
        public int? CatalogCategoriaContabilGestaoId { get; set; }
        public CatalogCategoriaContabilGestao CategoriaContabilGestao { get; set; }
        [Logger("Categoria Atividade", "Atividade.Nome")]
        public int? CatalogAtividadeId { get; set; }
        public CatalogAtividade Atividade { get; set; }

        [NotMapped]
        protected Dictionary<CategoriaContabil, string> categoriasContabeis = new Dictionary<CategoriaContabil, string> {
            { CategoriaContabil.ST, "Serviços de Terceiros" },
            { CategoriaContabil.MC, "Materiais de Consumo" },
            { CategoriaContabil.MP, "Materiais Permanentes e Equipamentos" },
            { CategoriaContabil.VD, "Viagens e Diárias" },
            { CategoriaContabil.OU, "Outros" }
        };

        [NotMapped]
        public string categoria {
            get {

                if(this.CategoriaContabilGestao != null) {
                    return this.CategoriaContabilGestao.Nome;
                }
                else if(this.CategoriaContabil != 0) {
                    return this.categoriasContabeis.GetValueOrDefault(this.CategoriaContabil);
                }
                return "";
            }
        }

    }


    public class AlocacaoRm {
        [Key]
        public int Id { get; set; }
        [Logger("Etapa", "Etapa.Desc")]
        public int? EtapaId { get; set; }
        public Etapa Etapa { get; set; }
        public int? ProjetoId { get; set; }
        [Logger("Recurso Material", "RecursoMaterial.Nome")]
        public int? RecursoMaterialId { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }

        [Logger("Empresa Financiadora", "EmpresaFinanciadora.NomeEmpresa")]
        public int? EmpresaFinanciadoraId { get; set; }
        [ForeignKey("EmpresaFinanciadoraId")]
        public Empresa EmpresaFinanciadora { get; set; }

        [Logger("Empresa Recebedora", "EmpresaRecebedora.NomeEmpresa")]
        public int? EmpresaRecebedoraId { get; set; }
        [ForeignKey("EmpresaRecebedoraId")]
        public Empresa EmpresaRecebedora { get; set; }
        [Logger("Quantidade")]
        public int Qtd { get; set; }
        [Logger("Justificativa")]
        public string Justificativa { get; set; }
    }
}