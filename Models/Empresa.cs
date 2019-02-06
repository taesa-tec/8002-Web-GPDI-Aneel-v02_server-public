using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class Empresa
    {   
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        public EmpresaClassificacao Classificacao { get; set; }
        [NotMapped]
        public string ClassificacaoValor { get => Enum.GetName(typeof(EmpresaClassificacao),Classificacao); }
        public int? CatalogEmpresaId { get; set; }
        public CatalogEmpresa CatalogEmpresa { get; set; }
        public string Cnpj { get; set; }
        public int? CatalogEstadoId { get; set; }
        [ForeignKey("CatalogEstadoId")]
        public CatalogEstado Estado { get; set; }
        public string RazaoSocial { get; set; }
    }
    public enum EmpresaClassificacao
    {   
        Proponente, Energia, Executora, Parceira
    }
}