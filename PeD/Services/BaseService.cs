using Microsoft.AspNetCore.Authorization;
using PeD.Data;

namespace PeD.Services
{
    public class BaseService
    {
        protected IAuthorizationService authorization;
        public readonly GestorDbContext _context;

        public BaseService(GestorDbContext context, IAuthorizationService authorizationService)
        {
            this.authorization = authorizationService;
            this._context = context;
        }
    }
}