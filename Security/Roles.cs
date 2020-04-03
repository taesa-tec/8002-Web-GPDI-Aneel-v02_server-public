using System.Collections.Generic;

namespace APIGestor.Security
{
    public class Roles
    {
        public const string AdminGestor = "Admin-APIGestor";
        public const string UserGestor = "User-APIGestor";
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