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

        public Resultado Incluir(Projeto dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Projeto";

            if (resultado.Inconsistencias.Count == 0 &&
                _context.Projetos.Where(
                p => p.Numero == dados.Numero).Count() > 0)
            {
                resultado.Inconsistencias.Add(
                    "Projeto com Número já cadastrado");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                dados.Tipo = obterTipoProjeto(dados.Numero.ToString());

                dados.Empresas = new List<Empresa>{new Empresa { CatalogEmpresaId = dados.CatalogEmpresaId }};
                
                _context.Projetos.Add(dados);
                
                _context.SaveChanges();
                
                resultado.Id = dados.Id.ToString();
                
            }

            return resultado;
        }
        private TipoProjeto obterTipoProjeto(string Numero){
            TipoProjeto Tipo = (TipoProjeto)1;
            if (Convert.ToInt32(Numero.Substring(0,4))>8999)
                Tipo = (TipoProjeto)2;
            return Tipo;
        }
        public Resultado Atualizar(Projeto dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Projeto";

            if (resultado.Inconsistencias.Count == 0)
            {
                Projeto Projeto = _context.Projetos.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if (Projeto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Projeto não encontrado");
                }
                CatalogStatus Status = _context.CatalogStatus.Where(
                    p => p.Id == dados.CatalogStatusId).FirstOrDefault();

                if (Status == null)
                {
                    resultado.Inconsistencias.Add(
                        "Status não encontrado");
                }
                else
                {
                    Projeto.Tipo = obterTipoProjeto(dados.Numero.ToString());
                    Projeto.Titulo = dados.Titulo==null ? Projeto.Titulo : dados.Titulo;
                    Projeto.TituloDesc = dados.TituloDesc==null ? Projeto.TituloDesc : dados.TituloDesc;
                    Projeto.Numero = dados.Numero==null ? Projeto.Numero : dados.Numero;
                    Projeto.CatalogEmpresaId = dados.CatalogEmpresaId==null ? Projeto.CatalogEmpresaId : dados.CatalogEmpresaId;
                    Projeto.CatalogSegmentoId = dados.CatalogSegmentoId==null ? Projeto.CatalogSegmentoId : dados.CatalogSegmentoId;
                    Projeto.AvaliacaoInicial = dados.AvaliacaoInicial==null ? Projeto.AvaliacaoInicial : dados.AvaliacaoInicial;
                    Projeto.CompartResultados = Enum.IsDefined(typeof(CompartResultados), dados.CompartResultados)? Projeto.CompartResultados : dados.CompartResultados;
                    Projeto.Motivacao = dados.Motivacao==null ? Projeto.Motivacao : dados.Motivacao;
                    Projeto.Originalidade = dados.Originalidade==null ? Projeto.Originalidade : dados.Originalidade;
                    Projeto.Aplicabilidade = dados.Aplicabilidade==null ? Projeto.Aplicabilidade : dados.Aplicabilidade;
                    Projeto.Relevancia = dados.Relevancia==null ? Projeto.Relevancia : dados.Relevancia;
                    Projeto.Razoabilidade = dados.Razoabilidade==null ? Projeto.Razoabilidade : dados.Razoabilidade;
                    Projeto.Pesquisas = dados.Pesquisas==null ? Projeto.Pesquisas : dados.Pesquisas;
                    
                    Empresa empresa = _context.Empresas.Where(e=>e.ProjetoId==dados.Id)
                                                .Where(e=>e.Classificacao==0).FirstOrDefault();
                    if (empresa!=null){
                        empresa.CatalogEmpresaId = dados.CatalogEmpresaId;
                    }else{
                        Projeto.Empresas = new List<Empresa>{new Empresa { CatalogEmpresaId = dados.CatalogEmpresaId }};
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
        public Resultado ValidaDadosData(Projeto dados)
        {
            var resultado = new Resultado();

            if (dados == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Projeto");
            }
            else{
                if (dados.Id<=0)
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Id do Projeto");
                }
                DateTime DataInicio;
                if (!DateTime.TryParse(dados.DataInicio.ToString(), out DataInicio))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a data de Início do Projeto");
                }
            }
            return resultado;
        }
        public Resultado AtualizaDataInicio(Projeto dados)
        {
            Resultado resultado = ValidaDadosData(dados);
            resultado.Acao = "Atualização de Data Início do Projeto";

            if (resultado.Inconsistencias.Count == 0)
            {
                Projeto Projeto = _context.Projetos
                        .Include("CatalogEmpresa")
                        .Where( p => p.Id == dados.Id).FirstOrDefault();

                if (Projeto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Projeto não encontrado");
                }
                else
                {
                    Projeto.DataInicio = dados.DataInicio;
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