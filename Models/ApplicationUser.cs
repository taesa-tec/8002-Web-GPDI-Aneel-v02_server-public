using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace APIGestor.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UserStatus Status { get; set; }
        public string NomeCompleto { get; set; }
        public int? CatalogEmpresaId;
        [ForeignKey("CatalogEmpresaId")]
        public CatalogEmpresa CatalogEmpresa { get; set; }
        public string RazaoSocial { get; set; }
        public byte[] FotoPerfil { get; set; }
        public string Role { get; set; }
        public string CPF { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
    public enum UserStatus
    {
        Inativo,
        Ativo
        
    }
}