using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Catalogos;
using PeD.Core.Models.Demandas;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
using PeD.Data.Builders;
using CategoriaContabil = PeD.Core.Models.Catalogos.CategoriaContabil;
using Contrato = PeD.Core.Models.Contrato;
using ProdutoTipo = PeD.Core.Models.Catalogos.ProdutoTipo;

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
            AddEntities(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        protected void AddEntities(ModelBuilder builder)
        {
            #region Global

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

            #endregion

            builder.Entity<Fornecedor>(); //.ToTable("Empresas");
            builder.Entity<Captacao>(eb =>
            {
                eb.Property(c => c.CreatedAt).HasDefaultValueSql("getdate()");
                eb.HasOne(c => c.ContratoSugerido).WithMany()
                    .HasForeignKey("ContratoSugeridoId")
                    .IsRequired(false);
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

            #region Proposta

            builder.Entity<Proposta>(_builder =>
            {
                _builder.HasIndex(p => new {p.CaptacaoId, p.FornecedorId}).IsUnique();
                _builder.HasOne(p => p.Relatorio);
                _builder.HasOne(p => p.Contrato).WithOne(c => c.Proposta);
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
            builder.Entity<PeD.Core.Models.Propostas.PropostaContrato>();
            builder.Entity<PeD.Core.Models.Propostas.PropostaContratoRevisao>(b =>
            {
                b.HasOne(c => c.Proposta).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Escopo>();
            builder.Entity<Meta>();
            builder.Entity<Etapa>(b =>
            {
                b.Property(e => e.Meses).HasConversion(
                    meses => JsonConvert.SerializeObject(meses),
                    meses => JsonConvert.DeserializeObject<List<int>>(meses));
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
                b.Property(e => e.HoraMeses).HasConversion(
                    meses => JsonConvert.SerializeObject(meses),
                    meses => JsonConvert.DeserializeObject<Dictionary<short, short>>(meses));
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

            #endregion

            builder.Entity<CaptacaoInfo>().ToView("CaptacoesView");
        }
    }
}