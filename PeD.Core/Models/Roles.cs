using System.Collections.Generic;

namespace PeD.Core.Models
{
    public class Roles
    {
        public const string Administrador = "Administrador";
        public const string User = "User";
        public const string Suprimento = "Suprimento";
        public const string Fornecedor = "Fornecedor";
        public const string Colaborador = "Colaborador";

        protected static List<string> _allRoles = new List<string>()
        {
            Roles.Administrador,
            Roles.User,
            Roles.Colaborador,
            Roles.Suprimento,
            Roles.Fornecedor
        };

        public static List<string> AllRoles => _allRoles;
    }
}