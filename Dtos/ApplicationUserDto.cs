using System;
using System.Collections.Generic;
using APIGestor.Models;
using APIGestor.Models.Catalogs;

namespace APIGestor.Dtos
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }

        public string StatusValor { get; set; }
        public string NomeCompleto { get; set; }
        public string Cargo { get; set; }
        public int? CatalogEmpresaId;
        public CatalogEmpresa CatalogEmpresa { get; set; }
        public string RazaoSocial { get; set; }
        public string FotoPerfil { get; set; }
        public string Role { get; set; }
        public List<string> Roles { get; set; }
        public string CPF { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}