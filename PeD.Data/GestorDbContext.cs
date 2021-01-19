using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Catalogos;
using PeD.Core.Models.Demandas;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
using PeD.Data.Builders;
using CategoriaContabil = PeD.Core.Models.Catalogos.CategoriaContabil;
using Contrato = PeD.Core.Models.Contrato;

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
            builder.Entity<FaseTipoDetalhado>().Seed();
            builder.Entity<Contrato>().ToTable("Contratos");
            builder.Entity<Clausula>().ToTable("Clausulas");

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

            builder.Entity<CaptacaoContrato>(b =>
            {
                b.ToTable("CaptacaoContratos")
                    .HasKey(cc => new {cc.CaptacaoId, cc.ContratoId});
                b.HasOne(cc => cc.Captacao).WithMany(c => c.Contratos).HasForeignKey("CaptacaoId")
                    .OnDelete(DeleteBehavior.Restrict);
                b.HasOne(cc => cc.Contrato).WithMany().HasForeignKey("ContratoId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<CaptacaoFornecedor>()
                .ToTable("CaptacoesFornecedores")
                .HasKey(a => new {a.FornecedorId, PropostaConfiguracaoId = a.CaptacaoId});

            builder.Entity<CaptacaoSugestaoFornecedor>()
                .ToTable("CaptacaoSugestoesFornecedores")
                .HasKey(a => new {a.FornecedorId, a.CaptacaoId});

            builder.Entity<Proposta>(builder =>
            {
                builder.HasIndex(p => new {p.CaptacaoId, p.FornecedorId}).IsUnique();
                builder.ToTable("Propostas");
            });

            builder.Entity<CaptacaoInfo>().ToView("CaptacoesView");
        }
    }
}