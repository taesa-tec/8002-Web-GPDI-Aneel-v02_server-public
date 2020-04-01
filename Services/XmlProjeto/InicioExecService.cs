using System.Linq;
using APIGestor.Data;
using APIGestor.Models.Projetos;
using APIGestor.Models.Projetos.Xmls;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Services.XmlProjeto
{
    public class XmlInicioExecService : IXmlService<InicioExecucao>
    {
        private GestorDbContext _context;
        public XmlInicioExecService(GestorDbContext context)
        {
            _context = context;
        }
        public Resultado ValidaXml(int ProjetoId)
        {
            Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();
            var resultado = new Resultado();
            resultado.Acao = "Validação de dados";
            if (projeto.Codigo == null)
                resultado.Inconsistencias.Add("Código do Projeto não gerado");
            return resultado;
        }
        public InicioExecucao GerarXml(int ProjetoId, string Versao, string UserId)
        {
            InicioExecucao Inicio = new InicioExecucao();
            Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();

            Inicio.PD_InicioExecProjeto = new PD_InicioExecProjeto
            {
                Projeto = new InicioProjeto
                {
                    CodProjeto = projeto.Codigo,
                    DataIniProjeto = projeto.DataInicio.ToString(),
                    DirPropIntProjeto = projeto.CompartResultadosValor
                }

            };
            return Inicio;
        }
    }
}
