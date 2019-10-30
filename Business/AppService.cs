using APIGestor.Authorizations;
using APIGestor.Data;
using APIGestor.Models;
using APIGestor.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APIGestor.Business
{

    public class AppService : BaseGestorService
    {
        public AppService(GestorDbContext context, IAuthorizationService authorization, LogService logService) : base(context, authorization, logService)
        {
        }

        public void setOption(string name, object value)
        {
            var option = _context.SystemOptions.FirstOrDefault(opt => opt.Key == name);
            if (option == null)
            {
                option = new SystemOption();
                option.Key = name;
                _context.Add(option);
            }
            option.setValue(value);
            _context.SaveChanges();
        }
        public T getOption<T>(string name)
        {
            var option = _context.SystemOptions.FirstOrDefault(opt => opt.Key == name);
            if (option != null)
            {
                return option.ToObject<T>();
            }
            return default(T);
        }

    }
}