using System.Collections.Generic;

namespace PeD.Core.Models
{
    public class Roles
    {
        public const string AdminGestor = "Admin-PeD";
        public const string UserGestor = "User-PeD";
        public const string Suprimento = "Suprimento";
        public const string Fornecedor = "Fornecedor";

        protected static List<string> _allRoles = new List<string>()
        {
            Roles.AdminGestor,
            Roles.UserGestor,
            Roles.Suprimento,
            Roles.Fornecedor
        };

        public static List<string> AllRoles
        {
            get { return _allRoles; }
        }
    }
}