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
            authorization = authorizationService;
            _context = context;
        }
    }
}