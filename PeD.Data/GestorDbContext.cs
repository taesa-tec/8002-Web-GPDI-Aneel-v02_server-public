using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Catalogos;
using PeD.Core.Models.Demandas;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Models.Propostas;
using PeD.Data.Builders;
using Alocacao = PeD.Core.Models.Projetos.Alocacao;
using AlocacaoRh = PeD.Core.Models.Propostas.AlocacaoRh;
using AlocacaoRhHorasMes = PeD.Core.Models.Projetos.AlocacaoRhHorasMes;
using Empresa = PeD.Core.Models.Propostas.Empresa;
using Escopo = PeD.Core.Models.Propostas.Escopo;
using Etapa = PeD.Core.Models.Propostas.Etapa;
using EtapaProdutos = PeD.Core.Models.Propostas.EtapaProdutos;
using ItemAjuda = PeD.Core.Models.Sistema.ItemAjuda;
using Meta = PeD.Core.Models.Propostas.Meta;
using Orcamento = PeD.Core.Models.Projetos.Orcamento;
using PlanoTrabalho = PeD.Core.Models.Propostas.PlanoTrabalho;
using Produto = PeD.Core.Models.Propostas.Produto;
using RecursoHumano = PeD.Core.Models.Propostas.RecursoHumano;
using RecursoMaterial = PeD.Core.Models.Propostas.RecursoMaterial;
using Risco = PeD.Core.Models.Propostas.Risco;

namespace PeD.Data
{
    public class GestorDbContext : IdentityDbContext<ApplicationUser>
    {
        #region DbSet's

        public DbSet<FileUpload> Files { get; set; }

        public DbSet<Core.Models.Empresa> Empresas { get; set; }
        public DbSet<Tema> Temas { get; set; }
        public DbSet<FaseCadeiaProduto> ProdutoFasesCadeia { get; set; }

        // Projeto Gestão

        public DbSet<CategoriaContabil> CategoriasContabeis { get; set; }
        public DbSet<CategoriaContabilAtividade> CategoriaContabilAtividades { get; set; }

        /* Demandas */

        #region Demandas

        public DbSet<Demanda> Demandas { get; set; }
        public DbSet<DemandaComentario> DemandaComentarios { get; set; }
        public DbSet<DemandaFormFile> DemandaFormFiles { get; set; }
        public DbSet<DemandaFormValues> DemandaFormValues { get; set; }
        public DbSet<DemandaFormHistorico> DemandaFormHistoricos { get; set; }
        public DbSet<SystemOption> SystemOptions { get; set; }
        public DbSet<DemandaFile> DemandaFiles { get; set; }
        public DbSet<DemandaLog> DemandaLogs { get; set; }

        #endregion

        //public DbSet<RegistroFinanceiro> RegistroFinanceiros { get; set; }
        //public DbSet<RegistroFinanceiroRh> RegistroFinanceirosRh { get; set; }
        //public DbSet<RegistroFinanceiroRm> RegistroFinanceirosRm { get; set; }

        #endregion

        public GestorDbContext(DbContextOptions<GestorDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Disable Cascate Delete
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            modelBuilder.Entity<Demanda>(_d => { _d.Property(d => d.CreatedAt).HasDefaultValueSql("getdate()"); });
            modelBuilder.Entity<DemandaComentario>(_dc =>
            {
                _dc.Property(dc => dc.CreatedAt).HasDefaultValueSql("getdate()");
            });
            CommonContext(modelBuilder);
            CaptacaoContext(modelBuilder);
            PropostaContext(modelBuilder);
            ProjetoContext(modelBuilder);
            ProjetoRelatorioContext(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        protected void CommonContext(ModelBuilder builder)
        {
            builder.Entity<Tema>().Config();
            builder.Entity<Segmento>().Config();
            builder.Entity<Estado>().Config();
            builder.Entity<Pais>().Config();
            builder.Entity<Core.Models.Empresa>().Config();
            builder.Entity<CategoriaContabil>().Config();
            builder.Entity<CategoriaContabilAtividade>().Seed();
            builder.Entity<FaseCadeiaProduto>().Config();
            builder.Entity<ProdutoTipo>().Seed();
            builder.Entity<Contrato>().ToTable("Contratos");
            builder.Entity<Clausula>().ToTable("Clausulas");
            builder.Entity<FaseTipoDetalhado>().Config();
            builder.Entity<ItemAjuda>().Config();
            builder.Entity<Fornecedor>();
        }

        protected void CaptacaoContext(ModelBuilder builder)
        {
            builder.Entity<Captacao>(eb =>
            {
                eb.Property(c => c.CreatedAt).HasDefaultValueSql("getdate()");
                eb.HasOne(c => c.ContratoSugerido).WithMany()
                    .HasForeignKey("ContratoSugeridoId")
                    .IsRequired(false);

                eb.HasOne(c => c.EspecificacaoTecnicaFile).WithOne().OnDelete(DeleteBehavior.NoAction);

                eb.HasMany(c => c.Propostas).WithOne(p => p.Captacao);
                eb.ToTable("Captacoes");
            });
            builder.Entity<CaptacaoArquivo>().ToTable("CaptacaoArquivos");

            builder.Entity<CaptacaoFornecedor>()
                .ToTable("CaptacoesFornecedores")
                .HasKey(a => new {a.FornecedorId, PropostaConfiguracaoId = a.CaptacaoId});

            builder.Entity<CaptacaoSugestaoFornecedor>()
                .ToTable("CaptacaoSugestoesFornecedores")
                .HasKey(a => new {a.FornecedorId, a.CaptacaoId});

            builder.Entity<CaptacaoSubTema>();

            builder.Entity<CaptacaoInfo>().ToView("CaptacoesView");
        }

        protected void PropostaContext(ModelBuilder builder)
        {
            #region Proposta

            builder.Entity<Proposta>(_builder =>
            {
                _builder.HasIndex(p => new {p.CaptacaoId, p.FornecedorId}).IsUnique();
                _builder.HasOne(p => p.Relatorio);
                _builder.HasOne(p => p.Contrato).WithOne(c => c.Proposta);
                _builder.Property(p => p.Guid).HasDefaultValueSql("NEWID()");
                _builder.ToTable("Propostas");
            });
            builder.Entity<PropostaArquivo>(b =>
            {
                b.HasKey(pa => new {pa.PropostaId, pa.ArquivoId});
                b.ToTable("PropostasArquivos");
            });


            builder.Entity<Relatorio>(builder =>
            {
                builder.HasOne(r => r.Proposta)
                    .WithMany(p => p.HistoricoRelatorios)
                    .HasForeignKey(r => r.PropostaId);
                builder.Property(r => r.Validacao).HasConversion(
                    validacao => JsonConvert.SerializeObject(validacao),
                    validacao => JsonConvert.DeserializeObject<ValidationResult>(validacao));
            });
            builder.Entity<Empresa>(b => { b.Property(e => e.Funcao).HasConversion<string>(); });
            builder.Entity<PropostaContrato>();
            builder.Entity<PropostaContratoRevisao>(b =>
            {
                b.HasOne(c => c.Proposta).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Escopo>();
            builder.Entity<Meta>();
            builder.Entity<Etapa>(b =>
            {
                b.Property(e => e.Meses).JsonConversion();


                b.HasOne(e => e.Produto).WithMany().HasForeignKey(e => e.ProdutoId).OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<EtapaProdutos>(b =>
            {
                b.HasOne(ep => ep.Produto).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(ep => ep.Etapa).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<PlanoTrabalho>();
            builder.Entity<Produto>();
            builder.Entity<RecursoHumano>(b =>
            {
                b.HasOne(r => r.Proposta).WithMany(p => p.RecursosHumanos).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(r => r.Empresa).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<RecursoMaterial>(b =>
            {
                b.HasOne(r => r.Proposta).WithMany(p => p.RecursosMateriais).OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<AlocacaoRh>(b =>
            {
                b.HasOne(a => a.EmpresaFinanciadora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Etapa).WithMany(e => e.RecursosHumanosAlocacoes).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Proposta)
                    .WithMany(e => e.RecursosHumanosAlocacoes)
                    .HasForeignKey(a => a.PropostaId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Recurso).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<PeD.Core.Models.Propostas.AlocacaoRhHorasMes>(b =>
            {
                b.HasKey(br => new {br.AlocacaoRhId, br.Mes});
                b.ToTable("PropostasAlocacaoRhHorasMeses");
            });
            builder.Entity<AlocacaoRm>(b =>
            {
                b.HasOne(a => a.Etapa).WithMany(e => e.RecursosMateriaisAlocacoes).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.EmpresaRecebedora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.EmpresaFinanciadora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Proposta)
                    .WithMany(e => e.RecursosMateriaisAlocacoes)
                    .HasForeignKey(a => a.PropostaId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Recurso).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Risco>();
            builder.Entity<ContratoComentario>(b => { b.ToTable("ContratoComentarios"); });
            builder.Entity<PlanoComentario>(b => { b.ToTable("PlanoComentarios"); });
            builder.Entity<PlanoComentarioFile>(b => { b.HasKey(pf => new {pf.ComentarioId, pf.FileId}); });
            builder.Entity<ContratoComentarioFile>(b => { b.HasKey(pf => new {pf.ComentarioId, pf.FileId}); });
            builder.Entity<AlocacaoInfo>(b =>
            {
                b.Property(a => a.EmpresaFinanciadoraFuncao).HasConversion<string>();
                b.Property(a => a.EmpresaRecebedoraFuncao).HasConversion<string>();
                b.HasNoKey();
                b.ToView("PropostaAlocacoesView");
            });

            #endregion
        }

        protected void ProjetoContext(ModelBuilder builder)
        {
            builder.Entity<Projeto>(b =>
            {
                b.HasIndex(p => p.CaptacaoId).IsUnique();
                b.HasOne(p => p.Proposta).WithOne().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(p => p.PlanoTrabalhoFile).WithOne().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(p => p.Contrato).WithOne().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(p => p.EspecificacaoTecnicaFile).WithOne().OnDelete(DeleteBehavior.NoAction);
                b.Property(p => p.SegmentoId).HasDefaultValue("G");
                b.Property(p => p.Compartilhamento).HasConversion<string>();
                b.ToTable("Projetos");
            });
            builder.Entity<ProjetoArquivo>(b =>
            {
                b.HasKey(pa => new {pa.ProjetoId, pa.ArquivoId});
                b.ToTable("ProjetoArquivos");
            });


            builder.Entity<ProjetoSubTema>(b =>
            {
                b.HasKey(e => new {e.ProjetoId, e.SubTemaId});
                b.ToTable("ProjetosSubtemas");
            });
            builder.Entity<Core.Models.Projetos.Empresa>();
            builder.Entity<Core.Models.Projetos.Escopo>();
            builder.Entity<Core.Models.Projetos.Meta>();

            builder.Entity<Core.Models.Projetos.Etapa>(b =>
            {
                b.Property(e => e.Meses).JsonConversion();
                b.HasOne(e => e.Produto).WithMany().HasForeignKey(e => e.ProdutoId).OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Core.Models.Projetos.EtapaProdutos>(b =>
            {
                b.HasOne(ep => ep.Produto).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(ep => ep.Etapa).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Core.Models.Projetos.PlanoTrabalho>();
            builder.Entity<Core.Models.Projetos.Produto>();
            builder.Entity<Core.Models.Projetos.RecursoHumano>(b =>
            {
                b.HasOne(r => r.Projeto).WithMany(p => p.RecursosHumanos).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(r => r.Empresa).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Core.Models.Projetos.RecursoMaterial>(b =>
            {
                b.HasOne(r => r.Projeto).WithMany(p => p.RecursosMateriais).OnDelete(DeleteBehavior.NoAction);
            });


            builder.Entity<Alocacao>(b =>
            {
                b.HasDiscriminator(a => a.Tipo);
                b.HasOne(a => a.EmpresaFinanciadora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Etapa).WithMany(e => e.Alocacoes).HasForeignKey(a => a.EtapaId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Projeto).WithMany(p => p.Alocacoes).HasForeignKey(a => a.ProjetoId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.ToTable("ProjetosRecursosAlocacoes");
            });
            builder.Entity<Core.Models.Projetos.AlocacaoRh>(b =>
            {
                b.HasMany(b => b.HorasMeses).WithOne().HasForeignKey(a => a.AlocacaoRhId);
                b.HasOne(a => a.RecursoHumano).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<AlocacaoRhHorasMes>(b =>
            {
                b.HasKey(b => new {b.AlocacaoRhId, b.Mes});
                b.ToTable("ProjetosAlocacaoRhHorasMeses");
            });
            builder.Entity<Core.Models.Projetos.RecursoMaterial.AlocacaoRm>(b =>
            {
                b.HasOne(a => a.EmpresaRecebedora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.EmpresaFinanciadora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.RecursoMaterial).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Core.Models.Projetos.Risco>();

            builder.Entity<RegistroFinanceiro>(b =>
            {
                b.HasOne(r => r.Projeto).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(r => r.Etapa).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(r => r.Financiadora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasMany(r => r.Observacoes).WithOne().HasForeignKey(o => o.RegistroId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.Property(r => r.Tipo).HasMaxLength(200);
                b.Property(r => r.Status).HasConversion<string>();
                b.Property(r => r.TipoDocumento).HasConversion<string>();

                b.ToTable("ProjetosRegistrosFinanceiros");
                b.HasDiscriminator(r => r.Tipo);
            });
            builder.Entity<RegistroFinanceiroRh>(b =>
            {
                b.HasOne(r => r.RecursoHumano).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<RegistroFinanceiroRm>(b =>
            {
                b.HasOne(r => r.RecursoMaterial).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(r => r.Recebedora).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<RegistroObservacao>(b => { b.ToTable("ProjetosRegistrosFinanceirosObservacoes"); });

            builder.Entity<ProjetoXml>(b => { b.Property(l => l.Tipo).HasConversion<string>(); });
            builder.Entity<RegistroFinanceiroInfo>(b =>
            {
                b.Property(r => r.Status).HasConversion<string>();
                b.Property(r => r.TipoDocumento).HasConversion<string>();
                b.ToView("RegistrosFinanceirosView");
            });
            builder.Entity<Orcamento>(b =>
            {
                b.HasNoKey();
                b.ToView("ProjetoOrcamentoView");
            });
        }

        protected void ProjetoRelatorioContext(ModelBuilder builder)
        {
            builder.Entity<RelatorioFinal>(b =>
            {
                b.ToTable("ProjetosRelatoriosFinais");
                b.HasOne(r => r.Projeto).WithOne();
            });
            builder.Entity<Apoio>(b =>
            {
                b.ToTable("ProjetosRelatoriosApoios");
                b.Property(a => a.Tipo).HasConversion<string>();
            });
            builder.Entity<Capacitacao>(b =>
            {
                b.ToTable("ProjetosRelatoriosCapacitacoes");
                b.Property(i => i.Tipo).HasConversion<string>();
                b.HasOne(c => c.Recurso).WithMany().HasForeignKey(c => c.RecursoId).OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<IndicadorEconomico>(b =>
            {
                b.ToTable("ProjetosRelatoriosIndicadoresEconomicos");
                b.Property(i => i.Tipo).HasConversion<string>();
            });
            builder.Entity<ProducaoCientifica>(b =>
            {
                b.ToTable("ProjetosRelatoriosProducoesCientificas");
                b.Property(i => i.Tipo).HasConversion<string>();
            });
            builder.Entity<PropriedadeIntelectual>(b =>
            {
                b.ToTable("ProjetosRelatoriosPropriedadesIntelectuais");

                b.Property(i => i.Tipo).HasConversion<string>();
            });
            builder.Entity<PropriedadeIntelectualInventor>(b =>
            {
                b.ToTable("ProjetosRelatoriosPropriedadesIntelectuaisInventores");
                b.HasOne(p => p.Recurso).WithMany().HasForeignKey(p => p.RecursoId).OnDelete(DeleteBehavior.NoAction);
                b.HasKey(i => new {i.PropriedadeId, i.RecursoId});
            });

            builder.Entity<RelatorioEtapa>(b =>
            {
                b.ToTable("ProjetosRelatoriosEtapas");
                b.HasOne(r => r.Etapa).WithOne(r => r.Relatorio).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Socioambiental>(b =>
            {
                b.ToTable("ProjetosRelatoriosSocioambiental");
                b.Property(i => i.Tipo).HasConversion<string>();
            });
        }
    }
}