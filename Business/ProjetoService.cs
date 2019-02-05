using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;

namespace APIGestor.Business
{
    public class ProjetoService
    {
        private GestorDbContext _context;

        public ProjetoService(GestorDbContext context)
        {
            _context = context;
        }

        public Projeto Obter(int id)
        {
            if (id>0)
            {
                return _context.Projetos
                    .Include(p => p.UsersProjeto)
                    .Include(p => p.CatalogEmpresa)
                    .Include(p => p.CatalogStatus)
                    //.Include(p => p.CatalogSegmento)
                    .Include("Tema.SubTemas")
                    .Include(p => p.Produtos)
                    .Include(p => p.Etapas)
                    .Include(p => p.Empresas)
                    .Include(p => p.RecursosHumanos)
                    .Include(p => p.AlocacoesRh)
                    .Include(p => p.RecursosMateriais)
                    .Include(p => p.AlocacoesRm)
                    .Where(
                        p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }

        public IEnumerable<Projeto> ListarTodos()
        {
            var Projetos = _context.Projetos
                .OrderBy(p => p.Titulo)
                .ToList();
            return Projetos;
        }

        public IEnumerable<UserProjeto> ObterUsuarios(int Id)
        {
            var UserProjetos = _context.UserProjetos
                .Include("ApplicationUser")
                .Include("CatalogUserPermissao")
                .Where(p => p.ProjetoId == Id)
                .ToList();
            return UserProjetos;
        }

        public Resultado Incluir(Projeto dadosProjeto)
        {
            Resultado resultado = DadosValidos(dadosProjeto);
            resultado.Acao = "Inclusão de Projeto";

            if (resultado.Inconsistencias.Count == 0 &&
                _context.Projetos.Where(
                p => p.Numero == dadosProjeto.Numero).Count() > 0)
            {
                resultado.Inconsistencias.Add(
                    "Projeto com Número já cadastrado");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                dadosProjeto.Tipo = obterTipoProjeto(dadosProjeto.Numero.ToString());

                dadosProjeto.Empresas = new List<Empresa>{new Empresa { CatalogEmpresaId = dadosProjeto.CatalogEmpresaId }};
                
                _context.Projetos.Add(dadosProjeto);
                
                _context.SaveChanges();
                
            }

            return resultado;
        }
        private TipoProjeto obterTipoProjeto(string Numero){
            TipoProjeto Tipo = (TipoProjeto)1;
            if (Numero.TrimStart('0').Substring(0,1)=="9")
                Tipo = (TipoProjeto)2;
            return Tipo;
        }
        public Resultado Atualizar(Projeto dadosProjeto)
        {
            Resultado resultado = DadosValidos(dadosProjeto);
            resultado.Acao = "Atualização de Projeto";

            if (resultado.Inconsistencias.Count == 0)
            {
                Projeto Projeto = _context.Projetos.Where(
                    p => p.Id == dadosProjeto.Id).FirstOrDefault();

                if (Projeto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Projeto não encontrado");
                }
                CatalogStatus Status = _context.CatalogStatus.Where(
                    p => p.Id == dadosProjeto.CatalogStatusId).FirstOrDefault();

                if (Status == null)
                {
                    resultado.Inconsistencias.Add(
                        "Status não encontrado");
                }
                else
                {
                    Projeto.Tipo = obterTipoProjeto(dadosProjeto.Numero.ToString());
                    Projeto.Titulo = dadosProjeto.Titulo==null ? Projeto.Titulo : dadosProjeto.Titulo;
                    Projeto.TituloDesc = dadosProjeto.TituloDesc==null ? Projeto.TituloDesc : dadosProjeto.TituloDesc;
                    Projeto.Numero = dadosProjeto.Numero==null ? Projeto.Numero : dadosProjeto.Numero;
                    Projeto.CatalogEmpresaId = dadosProjeto.CatalogEmpresaId==null ? Projeto.CatalogEmpresaId : dadosProjeto.CatalogEmpresaId;
                    Projeto.CatalogSegmentoId = dadosProjeto.CatalogSegmentoId==null ? Projeto.CatalogSegmentoId : dadosProjeto.CatalogSegmentoId;
                    Projeto.AvaliacaoInicial = dadosProjeto.AvaliacaoInicial==null ? Projeto.AvaliacaoInicial : dadosProjeto.AvaliacaoInicial;
                    Projeto.CompartResultados = dadosProjeto.CompartResultados==null ? Projeto.CompartResultados : dadosProjeto.CompartResultados;
                    Projeto.Motivacao = dadosProjeto.Motivacao==null ? Projeto.Motivacao : dadosProjeto.Motivacao;
                    Projeto.Originalidade = dadosProjeto.Originalidade==null ? Projeto.Originalidade : dadosProjeto.Originalidade;
                    Projeto.Aplicabilidade = dadosProjeto.Aplicabilidade==null ? Projeto.Aplicabilidade : dadosProjeto.Aplicabilidade;
                    Projeto.Relevancia = dadosProjeto.Relevancia==null ? Projeto.Relevancia : dadosProjeto.Relevancia;
                    Projeto.Razoabilidade = dadosProjeto.Razoabilidade==null ? Projeto.Razoabilidade : dadosProjeto.Razoabilidade;
                    Projeto.Pesquisas = dadosProjeto.Pesquisas==null ? Projeto.Pesquisas : dadosProjeto.Pesquisas;
                    
                    Empresa empresa = _context.Empresas.Where(e=>e.ProjetoId==dadosProjeto.Id)
                                                .Where(e=>e.Classificacao==0).FirstOrDefault();
                    if (empresa!=null){
                        empresa.CatalogEmpresaId = dadosProjeto.CatalogEmpresaId;
                    }else{
                        Projeto.Empresas = new List<Empresa>{new Empresa { CatalogEmpresaId = dadosProjeto.CatalogEmpresaId }};
                    }
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Projeto";

            Projeto Projeto = Obter(id);
            if (Projeto == null)
            {
                resultado.Inconsistencias.Add(
                    "Projeto não encontrado");
            }
            else
            {
                _context.Projetos.Remove(Projeto);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(Projeto Projeto)
        {
            var resultado = new Resultado();
            if (Projeto == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Projeto");
            }
            else
            {
                if (Projeto.CatalogEmpresaId<=0)
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a empresa Proponente do Projeto");
                }
                else{
                    CatalogEmpresa catalogEmpresa = _context.CatalogEmpresas
                                        .Where(e=>e.Id==Projeto.CatalogEmpresaId)
                                        .FirstOrDefault();
                    if (catalogEmpresa==null){
                        resultado.Inconsistencias.Add(
                        "CatalogEmpresaId não localizado");
                    }
                }
                if (String.IsNullOrWhiteSpace(Projeto.Titulo))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Título do Projeto");
                }
                if (String.IsNullOrWhiteSpace(Projeto.TituloDesc))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o título descrição do Projeto");
                }
                if (String.IsNullOrWhiteSpace(Projeto.Numero))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Número do Projeto");
                }
                if (Projeto.Numero.Length>5){
                    resultado.Inconsistencias.Add(
                        "Maximo de 5 digitos campo Número do Projeto");
                }
            }

            return resultado;
        }
        public Resultado ValidaDadosData(Projeto dadosProjeto)
        {
            var resultado = new Resultado();

            if (dadosProjeto == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Projeto");
            }
            else{
                if (dadosProjeto.Id<=0)
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Id do Projeto");
                }
                DateTime DataInicio;
                if (!DateTime.TryParse(dadosProjeto.DataInicio.ToString(), out DataInicio))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a data de Início do Projeto");
                }
            }
            return resultado;
        }
        public Resultado AtualizaDataInicio(Projeto dadosProjeto)
        {
            Resultado resultado = ValidaDadosData(dadosProjeto);
            resultado.Acao = "Atualização de Data Início do Projeto";

            if (resultado.Inconsistencias.Count == 0)
            {
                Projeto Projeto = _context.Projetos
                        .Include("CatalogEmpresa")
                        .Where( p => p.Id == dadosProjeto.Id).FirstOrDefault();

                if (Projeto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Projeto não encontrado");
                }
                else
                {
                    Projeto.DataInicio = dadosProjeto.DataInicio;
                    var codigo = GerarCodigoProjeto(Projeto);
                    Projeto.Codigo = codigo;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        public string GerarCodigoProjeto(Projeto projeto){
            var codigo = Enum.GetName(typeof(TipoProjeto),projeto.Tipo).ToString() + "-";
                codigo += projeto.CatalogEmpresa.Valor.ToString() + "-";
                codigo += projeto.Numero.ToString() + "/";
                codigo += projeto.DataInicio.HasValue ? projeto.DataInicio.Value.Year.ToString() : null;
            return codigo;
        }
    }
}