using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class Upload
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public string NomeArquivo { get; set; }
        public string Url { get; set; }
        public int? ProjetoId { get; set; }
        public int? TemaId { get; set; }
        public int? RegistroFinanceiroId { get; set; }
        public CategoriaUpload Categoria { get; set; }        
        public string UserId { get; set; }
        public DateTime Created { get; set; }
    }
    public enum CategoriaUpload
    {
        UsuarioPerfil = 1,
        Tema = 2,
        RegistroFinanceiro = 3,
        RelatorioFinalAnual = 4,
        RelatorioFinalAuditoria = 5
    }
}