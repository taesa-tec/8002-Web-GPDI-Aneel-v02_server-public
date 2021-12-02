using System;
using System.Collections.Generic;

namespace PeD.Core.ApiModels
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }

        public string NomeCompleto { get; set; }
        public string Cargo { get; set; }
        public int? EmpresaId;
        public string Empresa { get; set; }
        public string RazaoSocial { get; set; }
        public string FotoPerfil { get; set; }
        public string Role { get; set; }
        public List<string> Roles { get; set; }
        public string Cpf { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}