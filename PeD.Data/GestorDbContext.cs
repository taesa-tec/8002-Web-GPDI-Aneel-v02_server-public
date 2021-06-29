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
using PeD.Core.Models.Sistema;
using PeD.Data.Builders;
using CategoriaContabil = PeD.Core.Models.Catalogos.CategoriaContabil;
using CoExecutor = PeD.Core.Models.Propostas.CoExecutor;
using Contrato = PeD.Core.Models.Contrato;
using ContratoComentario = PeD.Core.Models.Propostas.ContratoComentario;
using ContratoComentarioFile = PeD.Core.Models.Propostas.ContratoComentarioFile;
using Escopo = PeD.Core.Models.Propostas.Escopo;
using Etapa = PeD.Core.Models.Propostas.Etapa;
using EtapaProdutos = PeD.Core.Models.Propostas.EtapaProdutos;
using ItemAjuda = PeD.Core.Models.Sistema.ItemAjuda;
using Meta = PeD.Core.Models.Propostas.Meta;
using PlanoComentario = PeD.Core.Models.Propostas.PlanoComentario;
using PlanoComentarioFile = PeD.Core.Models.Propostas.PlanoComentarioFile;
using PlanoTrabalho = PeD.Core.Models.Propostas.PlanoTrabalho;
using Produto = PeD.Core.Models.Propostas.Produto;
using ProdutoTipo = PeD.Core.Models.Catalogos.ProdutoTipo;
using RecursoHumano = PeD.Core.Models.Propostas.RecursoHumano;
using RecursoMaterial = PeD.Core.Models.Propostas.RecursoMaterial;
using Relatorio = PeD.Core.Models.Propostas.Relatorio;
using Risco = PeD.Core.Models.Propostas.Risco;

namespace PeD.Data
{
    public class GestorDbContext : IdentityDbContext<ApplicationUser>
    {
        public GestorDbContext(
            DbContextOptions<GestorDbContext> options) : base(options)
        {
        }

        #region DbSet's

        public DbSet<FileUpload> Files { get; set; }

        public DbSet<Empresa> Empresas { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Disable Cascate Delete
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            // Tema
            modelBuilder.Entity<Tema>().HasMany(p => p.SubTemas);

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
            builder.Entity<Empresa>().Config();
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
            builder.Entity<CoExecutor>();
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
            builder.Entity<RecursoHumano>();
            builder.Entity<RecursoMaterial>();
            builder.Entity<RecursoHumano.AlocacaoRh>(b =>
            {
                b.HasOne(a => a.EmpresaFinanciadora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Etapa).WithMany(e => e.RecursosHumanosAlocacoes).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Proposta)
                    .WithMany(e => e.RecursosHumanosAlocacoes)
                    .HasForeignKey(a => a.PropostaId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Recurso).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.Property(e => e.HoraMeses)
                    .JsonConversion(new ValueComparer<Dictionary<short, short>>((l1, l2) => l1.SequenceEqual(l2),
                        l => l.Aggregate(0, (i, s) => HashCode.Combine(i, s.GetHashCode()))));
            });
            builder.Entity<RecursoMaterial.AlocacaoRm>(b =>
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
            builder.Entity<Orcamento>(b =>
            {
                b.HasNoKey();
                b.ToView("ProjetoOrcamentoView");
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
                b.ToTable("Projetos");
            });
            builder.Entity<ProjetoArquivo>(b =>
            {
                b.HasKey(pa => new {pa.ProjetoId, pa.ArquivoId});
                b.ToTable("ProjetoArquivos");
            });

            builder.Entity<PeD.Core.Models.Projetos.CoExecutor>();
            builder.Entity<PeD.Core.Models.Projetos.Escopo>();
            builder.Entity<PeD.Core.Models.Projetos.Meta>();

            builder.Entity<PeD.Core.Models.Projetos.Etapa>(b =>
            {
                b.Property(e => e.Meses).JsonConversion();
                b.HasOne(e => e.Produto).WithMany().HasForeignKey(e => e.ProdutoId).OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<PeD.Core.Models.Projetos.EtapaProdutos>(b =>
            {
                b.HasOne(ep => ep.Produto).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(ep => ep.Etapa).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<PeD.Core.Models.Projetos.PlanoTrabalho>();
            builder.Entity<PeD.Core.Models.Projetos.Produto>();
            builder.Entity<PeD.Core.Models.Projetos.RecursoHumano>();
            builder.Entity<PeD.Core.Models.Projetos.RecursoMaterial>();


            builder.Entity<Core.Models.Projetos.Alocacao>(b =>
            {
                b.HasDiscriminator(a => a.Tipo);
                b.HasOne(a => a.CoExecutorFinanciador).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.EmpresaFinanciadora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Etapa).WithMany(e => e.Alocacoes).HasForeignKey(a => a.EtapaId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.Projeto).WithMany(p => p.Alocacoes).HasForeignKey(a => a.ProjetoId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.ToTable("ProjetosRecursosAlocacoes");
            });
            builder.Entity<PeD.Core.Models.Projetos.RecursoHumano.AlocacaoRh>(b =>
            {
                b.HasMany(b => b.HorasMeses).WithOne().HasForeignKey(a => a.AlocacaoRhId);
                b.HasOne(a => a.RecursoHumano).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Core.Models.Projetos.RecursoHumano.AlocacaoRhHorasMes>(b =>
            {
                b.HasKey(b => new {b.AlocacaoRhId, b.Mes});
                b.ToTable("ProjetosAlocacaoRhHorasMeses");
            });
            builder.Entity<PeD.Core.Models.Projetos.RecursoMaterial.AlocacaoRm>(b =>
            {
                b.HasOne(a => a.EmpresaRecebedora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.EmpresaFinanciadora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(a => a.RecursoMaterial).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<PeD.Core.Models.Projetos.Risco>();

            builder.Entity<RegistroFinanceiro>(b =>
            {
                b.HasOne(r => r.Projeto).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(r => r.Etapa).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(r => r.Financiadora).WithMany().OnDelete(DeleteBehavior.NoAction);
                b.HasOne(r => r.CoExecutorFinanciador).WithMany().OnDelete(DeleteBehavior.NoAction);
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
                b.HasOne(r => r.CoExecutorRecebedor).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<RegistroObservacao>(b => { b.ToTable("ProjetosRegistrosFinanceirosObservacoes"); });

            builder.Entity<ProjetoXml>(b => { b.Property(l => l.Tipo).HasConversion<string>(); });
            builder.Entity<RegistroFinanceiroInfo>(b =>
            {
                b.Property(r => r.Status).HasConversion<string>();
                b.Property(r => r.TipoDocumento).HasConversion<string>();
                b.ToView("RegistrosFinanceirosView");
            });
        }

        protected void ProjetoRelatorioContext(ModelBuilder builder)
        {
            builder.Entity<Apoio>(b => { b.ToTable("ProjetosRelatoriosApoios"); });
            builder.Entity<Capacitacao>(b => { b.ToTable("ProjetosRelatoriosCapacitacoes"); });
            builder.Entity<IndicadorEconomico>(b => { b.ToTable("ProjetosRelatoriosIndicadoresEconomicos"); });
            builder.Entity<ProducaoCientifica>(b => { b.ToTable("ProjetosRelatoriosProducoesCientificas"); });
            builder.Entity<PropriedadeIntelectual>(b => { b.ToTable("ProjetosRelatoriosPropriedadesIntelectuais"); });
            builder.Entity<PropriedadeIntelectualInventores>(b =>
            {
                b.ToTable("ProjetosRelatoriosPropriedadesIntelectuaisInventores");
                b.HasKey(i => new {i.PropriedadeId, i.RecursoId});
            });
            
            builder.Entity<RelatorioEtapa>(b => { b.ToTable("ProjetosRelatoriosEtapas"); });
            builder.Entity<RelatorioFinal>(b => { b.ToTable("ProjetosRelatoriosFinais"); });
            builder.Entity<Socioambiental>(b => { b.ToTable("ProjetosRelatoriosSocioambiental"); });
        }
    }
}