using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;

namespace APIGestor.Business
{
    public class TemaService
    {
        private GestorDbContext _context;

        public TemaService(GestorDbContext context)
        {
            _context = context;
        }
        public Tema ListarTema(int projetoId)
        {
            var Tema = _context.Temas
                .Include("CatalogTema")
                .Include("SubTemas.CatalogSubTema")
                .Include("Uploads")
                .Where(p => p.ProjetoId == projetoId)
                .FirstOrDefault();
            return Tema;
        }
        private List<TemaSubTema> MountTemaSubTema(Tema dados)
        {
            var subTemas = new List<TemaSubTema>();
            foreach (var t in dados.SubTemas)
            {
                subTemas.Add(new TemaSubTema { CatalogSubTemaId = t.CatalogSubTemaId, OutroDesc = t.OutroDesc });
            }
            return subTemas;
        }
        public Resultado Incluir(Tema dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Tema";
            if (dados.ProjetoId <= 0)
            {
                resultado.Inconsistencias.Add("Preencha o ProjetoId");
            }
            Projeto Projeto = _context.Projetos.Where(
                    p => p.Id == dados.ProjetoId).FirstOrDefault();

            if (Projeto == null)
            {
                resultado.Inconsistencias.Add("Projeto não localizado");
            }
            Tema Tema = _context.Temas.Where(
                p => p.ProjetoId == dados.ProjetoId).FirstOrDefault();

            if (Tema != null)
            {
                resultado.Inconsistencias.Add(
                    "Tema já cadastrado para esse projeto. Remova ou Atualize.");
            }
            if (resultado.Inconsistencias.Count == 0)
            {
                var tema = new Tema
                {
                    ProjetoId = dados.ProjetoId,
                    CatalogTemaId = dados.CatalogTemaId,
                    OutroDesc = dados.OutroDesc,
                    SubTemas = this.MountTemaSubTema(dados)
                };
                _context.Temas.Add(tema);
                _context.SaveChanges();
            }
            return resultado;
        }
        public Resultado Atualizar(Tema dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Tema";

            if (resultado.Inconsistencias.Count == 0)
            {
                Tema Tema = _context.Temas.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if (Tema == null)
                {
                    resultado.Inconsistencias.Add(
                        "Tema não encontrado");
                }
                else
                {
                    _context.TemaSubTemas.RemoveRange(_context.TemaSubTemas.Where(p => p.TemaId == dados.Id));
                    Tema.CatalogTemaId = dados.CatalogTemaId;
                    Tema.OutroDesc = dados.OutroDesc;
                    Tema.SubTemas = this.MountTemaSubTema(dados);
                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        private Resultado DadosValidos(Tema dados)
        {
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add("Preencha os Dados do Tema");
            }
            else
            {
                if (dados.CatalogTemaId <= 0)
                {
                    resultado.Inconsistencias.Add("Preencha o tema do projeto");
                }
                if (dados.SubTemas.Count <= 0)
                {
                    resultado.Inconsistencias.Add("Informe os subtemas");
                }
                foreach (var subtema in dados.SubTemas)
                {
                    CatalogSubTema CatalogSubTema = _context.CatalogSubTemas.Where(
                        p => p.SubTemaId == subtema.CatalogSubTemaId).Where(
                        p => p.CatalogTemaId == dados.CatalogTemaId).FirstOrDefault();

                    if (CatalogSubTema == null)
                    {
                        resultado.Inconsistencias.Add("Catalogo Tema ou Subtema não localizado ou não relacionados");
                    }
                }
            }
            return resultado;
        }
        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Tema";

            Tema Tema = _context.Temas.First(t => t.Id == id);
            if (Tema == null)
            {
                resultado.Inconsistencias.Add("Tema não encontrado");
            }
            else
            {   _context.TemaSubTemas.RemoveRange(_context.TemaSubTemas.Where(t=>t.TemaId == id));
                _context.Temas.Remove(Tema);
                _context.SaveChanges();
            }

            return resultado;
        }
    }
}