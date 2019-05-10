using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models {
    public enum CategoriaContabil {
        RH, ST, MC, MP, VD, OU
    }

    public class RecursoMaterial {
        [Key]
        public int Id { get; set; }
        public int? ProjetoId { get; set; }
        public string Nome { get; set; }
        public CategoriaContabil CategoriaContabil { get; set; }
        [NotMapped]
        public string CategoriaContabilValor { get => Enum.GetName(typeof(CategoriaContabil), CategoriaContabil); }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal ValorUnitario { get; set; }
        public string Especificacao { get; set; }
        public int? CatalogCategoriaContabilGestaoId { get; set; }
        public CatalogCategoriaContabilGestao CategoriaContabilGestao { get; set; }
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
                if (this.CategoriaContabil != 0) {
                    return this.categoriasContabeis.GetValueOrDefault(this.CategoriaContabil);
                }
                else if (this.CategoriaContabilGestao != null) {
                    return this.CategoriaContabilGestao.Nome;
                }
                return "";
            }
        }

    }


    public class AlocacaoRm {
        [Key]
        public int Id { get; set; }
        public int? EtapaId { get; set; }
        public Etapa Etapa { get; set; }
        public int? ProjetoId { get; set; }
        public int? RecursoMaterialId { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }

        public int? EmpresaFinanciadoraId { get; set; }
        [ForeignKey("EmpresaFinanciadoraId")]
        public Empresa EmpresaFinanciadora { get; set; }
        public int? EmpresaRecebedoraId { get; set; }
        [ForeignKey("EmpresaRecebedoraId")]
        public Empresa EmpresaRecebedora { get; set; }
        public int Qtd { get; set; }
        public string Justificativa { get; set; }
    }
}