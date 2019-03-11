using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace APIGestor.Business
{
    public class XmlInicioExecService
    {
        private GestorDbContext _context;
        public XmlInicioExecService(GestorDbContext context)
        {
            _context = context;
        }
        public Resultado ValidaXml(Projeto projeto)
        {
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
            var resultado = ValidaXml(projeto);
            if (resultado.Inconsistencias.Count == 0)
            {

                Inicio.PD_InicioExecProjeto = new PD_InicioExecProjeto
                {
                    Projeto = new InicioProjeto
                    {
                        CodProjeto = projeto.Codigo,
                        DataIniProjeto = projeto.DataInicio.ToString(),
                        DirPropIntProjeto = projeto.CompartResultadosValor
                    }

                };

            }
            return Inicio;
        }
    }
}
