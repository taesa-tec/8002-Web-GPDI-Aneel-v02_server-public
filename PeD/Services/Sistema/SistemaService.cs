using System.Collections.Generic;
using System.Linq;
using PeD.Core.Equipe;
using PeD.Core.Models;
using PeD.Data;

namespace PeD.Services.Sistema
{
    public class SistemaService
    {
        protected static EquipePeD EquipePeD { get; set; }
        protected GestorDbContext context;

        public SistemaService(GestorDbContext context)
        {
            this.context = context;
        }

        public void SetOption(string name, object value)
        {
            var option = context.SystemOptions.FirstOrDefault(opt => opt.Key == name);
            if (option == null)
            {
                option = new SystemOption();
                option.Key = name;
                context.Add(option);
            }

            option.setValue(value);
            context.SaveChanges();
        }

        public T GetOption<T>(string name)
        {
            var option = context.SystemOptions.FirstOrDefault(opt => opt.Key == name);
            if (option != null)
            {
                return option.ToObject<T>();
            }

            return default(T);
        }

        public EquipePeD GetEquipePeD()
        {
            if (SistemaService.EquipePeD != null)
            {
                return SistemaService.EquipePeD;
            }

            var EquipeOutros = GetOption<List<string>>("equipe-ped-outros");
            var Equipe = new EquipePeD()
            {
                Diretor = GetOption<string>("equipe-ped-diretor"),
                Gerente = GetOption<string>("equipe-ped-gerente"),
                Coordenador = GetOption<string>("equipe-ped-coordenador"),
            };
            SistemaService.EquipePeD = Equipe;
            return Equipe;
        }

        public object GetEquipePedUsers()
        {
            var equipe = GetEquipePeD();
            return new
            {
                Diretor = context.Users.FirstOrDefault(u => u.Id == equipe.Diretor),
                Gerente = context.Users.FirstOrDefault(u => u.Id == equipe.Gerente),
                Coordenador = context.Users.FirstOrDefault(u => u.Id == equipe.Coordenador),
            };
        }

        public void SetEquipePeD(string diretorId, string gerenteId, string coordenadorId)
        {
            SetOption("equipe-ped-diretor", diretorId);
            SetOption("equipe-ped-gerente", gerenteId);
            SetOption("equipe-ped-coordenador", coordenadorId);
            SistemaService.EquipePeD = null;
        }

        public void SetEquipePeD(EquipePeD equipePeD)
        {
            SetEquipePeD(equipePeD.Diretor, equipePeD.Gerente, equipePeD.Coordenador);
        }
    }
}