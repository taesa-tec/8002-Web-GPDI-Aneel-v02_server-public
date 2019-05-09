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
        private EtapaService _etapaService;

        public ProjetoService(GestorDbContext context, EtapaService etapaService)
        {
            _context = context;
            _etapaService = etapaService;
        }

        public Projeto Obter(int id)
        {
            if (id>0)
            {
                return _context.Projetos
                    .Include(p => p.UsersProjeto)
                    .Include(p => p.CatalogEmpresa)
                    .Include(p => p.CatalogStatus)
                    .Include(p => p.CatalogSegmento)
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
                .Include(p => p.CatalogStatus)
                .Include(p => p.CatalogEmpresa)
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
        public Resultado ProrrogarProjeto(Projeto dados)
        {
            var resultado = new Resultado();
            resultado.Acao = "Prorrogar projeto";
            var projeto = _context.Projetos.Include("Etapas").FirstOrDefault(p=>p.Id==dados.Id);
            if (projeto==null){
                resultado.Inconsistencias.Add("Projeto não localizado");
            }else{
                if (projeto.DataInicio==null){
                    resultado.Inconsistencias.Add("Data Inicio projeto não definida");
                }
                 if (projeto.Etapas.Count()==0){
                    resultado.Inconsistencias.Add("Projeto não possui etapas");
                }
            }
            if (resultado.Inconsistencias.Count()==0){
                var LastEtapa = _etapaService.AddDataEtapas(projeto.Etapas).LastOrDefault();
                if (LastEtapa.DataFim!=null){
                    var duracao = Math.Abs(12 * (LastEtapa.DataFim.Value.Year - dados.DataFim.Value.Year) + LastEtapa.DataFim.Value.Month - dados.DataFim.Value.Month);
                    var newDate = LastEtapa.DataFim.Value.AddMonths(duracao);
                    var newEtapa = new Etapa{
                        ProjetoId = projeto.Id,
                        Nome = "Prorrogação",
                        Desc = "Prorrogação",
                        Duracao = duracao,
                        DataInicio = LastEtapa.DataFim,
                        DataFim = newDate,
                        EtapaProdutos = _etapaService.MontEtapaProdutos(dados.Etapa)
                    };
                    _context.Etapas.Add(newEtapa);
                    _context.SaveChanges();
                    resultado.Id = newEtapa.Id.ToString();
                }
            }
            return resultado;
        }
        public Resultado Incluir(Projeto dados, string userId)
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
                // criar user projeto
                var userProjeto = new UserProjeto{
                    UserId = userId,
                    ProjetoId = dados.Id,
                    CatalogUserPermissaoId = 4
                };
                _context.UserProjetos.Add(userProjeto);
                _context.SaveChanges();
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
            //Resultado resultado = DadosValidos(dados);
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Projeto");
            }
            resultado.Acao = "Atualização de Projeto";

            if (resultado.Inconsistencias.Count == 0)
            {
                Projeto Projeto = _context.Projetos.Include("CatalogEmpresa").Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if (Projeto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Projeto não encontrado");
                }
                if (dados.CatalogStatusId!=null){
                    CatalogStatus Status = _context.CatalogStatus.Where(
                        p => p.Id == dados.CatalogStatusId).FirstOrDefault();

                    if (Status == null)
                    {
                        resultado.Inconsistencias.Add(
                            "Status não encontrado");
                    }
                }
                if (resultado.Inconsistencias.Count == 0)
                {
                    Projeto.CatalogStatusId = dados.CatalogStatusId==null ? Projeto.CatalogStatusId : dados.CatalogStatusId;
                    Projeto.Tipo = obterTipoProjeto(dados.Numero.ToString());
                    Projeto.Titulo = dados.Titulo==null ? Projeto.Titulo : dados.Titulo;
                    Projeto.TituloDesc = dados.TituloDesc==null ? Projeto.TituloDesc : dados.TituloDesc;
                    Projeto.Numero = dados.Numero==null ? Projeto.Numero : dados.Numero;
                    Projeto.CatalogEmpresaId = dados.CatalogEmpresaId==null ? Projeto.CatalogEmpresaId : dados.CatalogEmpresaId;
                    Projeto.CatalogSegmentoId = dados.CatalogSegmentoId==null ? Projeto.CatalogSegmentoId : dados.CatalogSegmentoId;
                    Projeto.AvaliacaoInicial = dados.AvaliacaoInicial==null ? Projeto.AvaliacaoInicial : dados.AvaliacaoInicial;
                    Projeto.CompartResultados = (dados.CompartResultados!=null && Enum.IsDefined(typeof(CompartResultados), dados.CompartResultados)) ? dados.CompartResultados : Projeto.CompartResultados;
                    Projeto.Motivacao = dados.Motivacao==null ? Projeto.Motivacao : dados.Motivacao;
                    Projeto.Originalidade = dados.Originalidade==null ? Projeto.Originalidade : dados.Originalidade;
                    Projeto.Aplicabilidade = dados.Aplicabilidade==null ? Projeto.Aplicabilidade : dados.Aplicabilidade;
                    Projeto.Relevancia = dados.Relevancia==null ? Projeto.Relevancia : dados.Relevancia;
                    Projeto.Razoabilidade = dados.Razoabilidade==null ? Projeto.Razoabilidade : dados.Razoabilidade;
                    Projeto.Pesquisas = dados.Pesquisas==null ? Projeto.Pesquisas : dados.Pesquisas;
                    
                    if (dados.CatalogEmpresaId!=null){
                        Empresa empresa = _context.Empresas.Where(e=>e.ProjetoId==dados.Id)
                                                    .Where(e=>e.Classificacao==0).FirstOrDefault();
                        if (empresa!=null){
                            empresa.CatalogEmpresaId = dados.CatalogEmpresaId;
                        }else{
                            Projeto.Empresas = new List<Empresa>{new Empresa { CatalogEmpresaId = dados.CatalogEmpresaId }};
                        }
                    }
                    var codigo = GerarCodigoProjeto(Projeto);
                    Projeto.Codigo = codigo;
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
                _context.UserProjetos.RemoveRange(_context.UserProjetos.Where(t=>t.ProjetoId == id));
                _context.Empresas.RemoveRange(_context.Empresas.Where(t=>t.ProjetoId == id));
                
                // Remove Etapas e associados
                foreach(var etapa in _context.Etapas.Where(t=>t.ProjetoId == id))
                {
                    _context.EtapaMeses.RemoveRange(_context.EtapaMeses.Where(t=>t.EtapaId == etapa.Id));
                    _context.EtapaProdutos.RemoveRange(_context.EtapaProdutos.Where(t=>t.EtapaId == etapa.Id));
                    _context.Etapas.Remove(etapa);
                }
                // Remove Temas e Arquivos
                foreach(var tema in _context.Temas.Where(t=>t.ProjetoId == id))
                {
                    _context.Uploads.RemoveRange(_context.Uploads.Where(t=>t.TemaId == tema.Id));
                    _context.TemaSubTemas.RemoveRange(_context.TemaSubTemas.Where(t=>t.TemaId == tema.Id));
                    _context.Temas.Remove(tema);
                }
                _context.AtividadesGestao.RemoveRange(_context.AtividadesGestao.Where(t=>t.ProjetoId == id));
                _context.Produtos.RemoveRange(_context.Produtos.Where(t=>t.ProjetoId == id));
                _context.AlocacoesRh.RemoveRange(_context.AlocacoesRh.Where(t=>t.ProjetoId == id));
                _context.RecursoHumanos.RemoveRange(_context.RecursoHumanos.Where(t=>t.ProjetoId == id));
                _context.AlocacoesRm.RemoveRange(_context.AlocacoesRm.Where(t=>t.ProjetoId == id));
                _context.RecursoMateriais.RemoveRange(_context.RecursoMateriais.Where(t=>t.ProjetoId == id));
                // Remove Registros Financeiros
                foreach(var registro in _context.RegistrosFinanceiros.Where(t=>t.ProjetoId == id).ToList())
                {
                    _context.Uploads.RemoveRange(_context.Uploads.Where(t=>t.RegistroFinanceiroId == registro.Id));
                    _context.RegistroObs.RemoveRange(_context.RegistroObs.Where(t=>t.RegistroFinanceiroId == registro.Id));
                    _context.RegistrosFinanceiros.Remove(registro);
                }
                _context.Uploads.RemoveRange(_context.Uploads.Where(t=>t.ProjetoId == id));

                // Remove Relatorio Final e Arquivos
                foreach(var rf in _context.RelatorioFinal.Where(t=>t.ProjetoId == id))
                {
                    _context.Uploads.RemoveRange(_context.Uploads.Where(t=>t.RelatorioFinalId == rf.Id));
                    _context.RelatorioFinal.Remove(rf);
                }
                // Remove Resultado Capacitação e Arquivos
                foreach(var rc in _context.ResultadosCapacitacao.Where(t=>t.ProjetoId == id))
                {
                    _context.Uploads.RemoveRange(_context.Uploads.Where(t=>t.ResultadoCapacitacaoId == rc.Id));
                    _context.ResultadosCapacitacao.Remove(rc);
                }
                _context.ResultadosEconomico.RemoveRange(_context.ResultadosEconomico.Where(t=>t.ProjetoId == id));
                _context.ResultadosInfra.RemoveRange(_context.ResultadosInfra.Where(t=>t.ProjetoId == id));
                _context.ResultadosIntelectual.RemoveRange(_context.ResultadosIntelectual.Where(t=>t.ProjetoId == id));
                
                // Remove Resultado Intelectual e associações
                foreach(var ri in _context.ResultadosIntelectual.Where(t=>t.ProjetoId == id))
                {
                    _context.ResultadoIntelectualInventores.RemoveRange(_context.ResultadoIntelectualInventores.Where(p => p.ResultadoIntelectualId == ri.Id));
                    _context.ResultadoIntelectualDepositantes.RemoveRange(_context.ResultadoIntelectualDepositantes.Where(p => p.ResultadoIntelectualId == ri.Id));   
                    _context.ResultadosIntelectual.Remove(ri);
                }
                
                // Remove Resultado Produção e Arquivos
                foreach(var rp in _context.ResultadosProducao.Where(t=>t.ProjetoId == id))
                {
                    _context.Uploads.RemoveRange(_context.Uploads.Where(t=>t.ResultadoProducaoId == rp.Id));
                    _context.ResultadosProducao.Remove(rp);
                }
                _context.ResultadosSocioAmbiental.RemoveRange(_context.ResultadosSocioAmbiental.Where(t=>t.ProjetoId == id));
                _context.LogProjetos.RemoveRange(_context.LogProjetos.Where(t=>t.ProjetoId == id));
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
                if (Projeto.Numero!=null)
                    if (Projeto.Numero.Length>5)
                        resultado.Inconsistencias.Add("Maximo de 5 digitos campo Número do Projeto");
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