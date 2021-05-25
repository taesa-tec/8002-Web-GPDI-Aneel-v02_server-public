using PeD.Core.Models.Projetos;
using TaesaCore.Interfaces;
using TaesaCore.Services;

namespace PeD.Services.Projetos
{
    public class ProjetoService : BaseService<Projeto>
    {
        public ProjetoService(IRepository<Projeto> repository) : base(repository)
        {
        }
    }
}