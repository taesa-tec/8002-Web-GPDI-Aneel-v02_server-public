﻿using System.Linq;
using APIGestor.Data;
using APIGestor.Models.Projetos;
using APIGestor.Models.Projetos.Xmls;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Services.Projetos.XmlProjeto
{
    public class XmlInteressePedService : IXmlService<InteresseExecucao>
    {
        private GestorDbContext _context;
        public XmlInteressePedService(GestorDbContext context)
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
        public InteresseExecucao GerarXml(int ProjetoId, string Versao, string UserId)
        {
            InteresseExecucao Interesse = new InteresseExecucao();
            Projeto projeto = _context.Projetos
                    .Include("CatalogEmpresa")
                    .Where(p => p.Id == ProjetoId)
                    .FirstOrDefault();
                Interesse.PD_InteresseProjeto = new PD_InteresseProjeto
                {
                    Projeto = new InteresseProjeto
                    {
                        CodProjeto = projeto.Codigo
                    }
                };
            return Interesse;
        }
    }
}