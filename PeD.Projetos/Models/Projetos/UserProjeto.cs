using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models;

namespace PeD.Projetos.Models.Projetos
{
    public class UserProjeto
    {
        private int _Id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _Id;
            set => _Id = value;
        }

        public string UserId{ get; set;}
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int ProjetoId { get; set; }
        public Projeto Projeto {get; set;}
        public int CatalogUserPermissaoId { get; set; }
        public CatalogUserPermissao CatalogUserPermissao { get; set; }

    }
    public class CatalogUserPermissao{
        private int _id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Valor { get; set; }
        public string Nome { get; set; }
    }
}