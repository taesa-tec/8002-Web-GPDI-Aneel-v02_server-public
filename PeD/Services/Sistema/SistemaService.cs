using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PeD.Core;
using PeD.Core.ApiModels;
using PeD.Core.Models;
using PeD.Data;

namespace PeD.Services.Sistema
{
    public class SistemaService
    {
        protected static EquipePeD EquipePeD { get; set; }
        protected GestorDbContext context;
        protected UserManager<ApplicationUser> _userManager;
        private IMapper _mapper;

        public SistemaService(GestorDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public void SetOption(string name, object value)
        {
            var option = context.SystemOptions.FirstOrDefault(opt => opt.Key == name);
            if (option == null)
            {
                option = new SystemOption() { Key = name };
                context.Add(option);
            }

            option.SetValue(value);
            context.SaveChanges();
        }

        public T GetOption<T>(string name)
        {
            var option = context.SystemOptions.FirstOrDefault(opt => opt.Key == name);
            return option != null ? option.ToObject<T>() : default;
        }

        public EquipePeD GetEquipePeD()
        {
            var diretor = GetOption<string>("equipe-ped-diretor");
            var gerente = GetOption<string>("equipe-ped-gerente");
            var coordenador = GetOption<string>("equipe-ped-coordenador");
            var equipeOutros = _userManager.GetUsersInRoleAsync(Roles.User).Result
                .Concat(_userManager.GetUsersInRoleAsync(Roles.Colaborador).Result)
                .Select(u => u.Id).ToList();
            var equipe = new EquipePeD()
            {
                Diretor = diretor,
                Gerente = gerente,
                Coordenador = coordenador,
                Outros = equipeOutros
            };
            EquipePeD = equipe;
            return EquipePeD;
        }

        public object GetEquipePedUsers()
        {
            var equipe = GetEquipePeD();
            return new
            {
                Diretor = _mapper.Map<ApplicationUserDto>(context.Users.FirstOrDefault(u => u.Id == equipe.Diretor)),
                Gerente = _mapper.Map<ApplicationUserDto>(context.Users.FirstOrDefault(u => u.Id == equipe.Gerente)),
                Coordenador =
                    _mapper.Map<ApplicationUserDto>(context.Users.FirstOrDefault(u => u.Id == equipe.Coordenador)),
                Outros = _mapper.Map<List<ApplicationUserDto>>(context.Users.Where(u => equipe.Outros.Contains(u.Id)))
            };
        }

        public void SetEquipePeD(string diretorId, string gerenteId, string coordenadorId)
        {
            SetOption("equipe-ped-diretor", diretorId);
            SetOption("equipe-ped-gerente", gerenteId);
            SetOption("equipe-ped-coordenador", coordenadorId);
            EquipePeD = null;
        }

        public void SetEquipePeD(EquipePeD equipePeD)
        {
            SetEquipePeD(equipePeD.Diretor, equipePeD.Gerente, equipePeD.Coordenador);
        }
    }
}