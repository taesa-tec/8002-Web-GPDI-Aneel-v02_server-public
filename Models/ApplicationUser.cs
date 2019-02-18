using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace APIGestor.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UserStatus Status { get; set; }
        [NotMapped]
        public string StatusValor { get => Enum.GetName(typeof(UserStatus),Status); }
        public string NomeCompleto { get; set; }
        public int? CatalogEmpresaId;
        [ForeignKey("CatalogEmpresaId")]
        public CatalogEmpresa CatalogEmpresa { get; set; }
        public string RazaoSocial { get; set; }
        public FotoPerfil FotoPerfil { get; set; }
        public string Role { get; set; }
        public string CPF { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
    public class FotoPerfil
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public byte[] File{ get; set; }

    }
    public enum UserStatus
    {
        Inativo,
        Ativo
        
    }
}