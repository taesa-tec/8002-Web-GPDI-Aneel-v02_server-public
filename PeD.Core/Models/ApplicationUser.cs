using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using PeD.Core.Models.Catalogos;

namespace PeD.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Status { get; set; }

        public string NomeCompleto { get; set; }
        public string Cargo { get; set; }
        public int? EmpresaId;
        [ForeignKey("EmpresaId")] public Empresa Empresa { get; set; }
        public string RazaoSocial { get; set; }
        [NotMapped] public string FotoPerfil => $"/avatar/{Id}.jpg";
        public string Role { get; set; }
        [NotMapped] public List<string> Roles { get; set; }
        public string Cpf { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}