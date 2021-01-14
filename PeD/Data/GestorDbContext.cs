using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using PeD.Models;
using PeD.Models.Captacao;
using PeD.Models.Catalogs;
using PeD.Models.Demandas;
using PeD.Models.Fornecedores;
using PeD.Models.Projetos;
using PeD.Models.Projetos.Resultados;

namespace PeD.Data
{
    public class GestorDbContext : IdentityDbContext<ApplicationUser>
    {
        public GestorDbContext(
            DbContextOptions<GestorDbContext> options) : base(options)
        {
        }

        #region DbSet's

        public DbSet<FotoPerfil> FotoPerfil { get; set; }
        public DbSet<LogProjeto> LogProjetos { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<CatalogUserPermissao> CatalogUserPermissoes { get; set; }
        public DbSet<CatalogStatus> CatalogStatus { get; set; }
        public DbSet<CatalogSegmento> CatalogSegmentos { get; set; }
        public DbSet<CatalogEmpresa> CatalogEmpresas { get; set; }
        public DbSet<CatalogEstado> CatalogEstados { get; set; }
        public DbSet<CatalogPais> CatalogPaises { get; set; }
        public DbSet<CatalogTema> CatalogTema { get; set; }
        public DbSet<CatalogSubTema> CatalogSubTemas { get; set; }
        public DbSet<CatalogProdutoFaseCadeia> CatalogProdutoFaseCadeia { get; set; }
        public DbSet<CatalogProdutoTipoDetalhado> CatalogProdutoTipoDetalhado { get; set; }
        public DbSet<UserProjeto> UserProjetos { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Etapa> Etapas { get; set; }
        public DbSet<EtapaProduto> EtapaProdutos { get; set; }
        public DbSet<Tema> Temas { get; set; }
        public DbSet<TemaSubTema> TemaSubTemas { get; set; }
        public DbSet<RecursoHumano> RecursoHumanos { get; set; }
        public DbSet<AlocacaoRh> AlocacoesRh { get; set; }
        public DbSet<RecursoMaterial> RecursoMateriais { get; set; }
        public DbSet<AlocacaoRm> AlocacoesRm { get; set; }
        public DbSet<RegistroFinanceiro> RegistrosFinanceiros { get; set; }
        public DbSet<RegistroObs> RegistroObs { get; set; }

        public DbSet<RelatorioFinal> RelatorioFinal { get; set; }
        public DbSet<ResultadoCapacitacao> ResultadosCapacitacao { get; set; }
        public DbSet<ResultadoProducao> ResultadosProducao { get; set; }
        public DbSet<ResultadoInfra> ResultadosInfra { get; set; }
        public DbSet<ResultadoIntelectual> ResultadosIntelectual { get; set; }
        public DbSet<ResultadoIntelectualInventor> ResultadoIntelectualInventores { get; set; }
        public DbSet<ResultadoIntelectualDepositante> ResultadoIntelectualDepositantes { get; set; }
        public DbSet<ResultadoSocioAmbiental> ResultadosSocioAmbiental { get; set; }
        public DbSet<ResultadoEconomico> ResultadosEconomico { get; set; }

        // Projeto Gestão
        public DbSet<AtividadesGestao> AtividadesGestao { get; set; }

        public DbSet<CatalogCategoriaContabilGestao> CatalogCategoriaContabilGestao { get; set; }
        public DbSet<CatalogAtividade> CatalogAtividade { get; set; }
        public DbSet<EtapaMes> EtapaMeses { get; set; }

        /* Demandas */
        public DbSet<Demanda> Demandas { get; set; }
        public DbSet<DemandaComentario> DemandaComentarios { get; set; }
        public DbSet<DemandaFormFile> DemandaFormFiles { get; set; }
        public DbSet<DemandaFormValues> DemandaFormValues { get; set; }
        public DbSet<DemandaFormHistorico> DemandaFormHistoricos { get; set; }
        public DbSet<SystemOption> SystemOptions { get; set; }
        public DbSet<FileUpload> Files { get; set; }
        public DbSet<DemandaFile> DemandaFiles { get; set; }
        public DbSet<DemandaLog> DemandaLogs { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Disable Cascate Delete
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            //Upload
            modelBuilder.Entity<Upload>()
                .Property(b => b.Created)
                .HasDefaultValueSql("getdate()");
            //Log Projeto
            modelBuilder.Entity<LogProjeto>()
                .Property(b => b.Created)
                .HasDefaultValueSql("getdate()");
            // Projeto
            modelBuilder.Entity<Projeto>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.CatalogStatus);
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.CatalogSegmento);
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.CatalogEmpresa);
            //modelBuilder.Entity<Projeto>()
            //  .HasOne(p => p.Tema);
            modelBuilder.Entity<Projeto>()
                .Property(b => b.Created)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.UsersProjeto);
            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.Produtos);
            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.RecursosHumanos);
            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.AlocacoesRh);
            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.RecursosMateriais);
            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.AlocacoesRm);
            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.Etapas);
            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.Empresas);

            // Etapa
            modelBuilder.Entity<Etapa>()
                .HasMany(p => p.EtapaProdutos);
            // Tema
            modelBuilder.Entity<CatalogTema>()
                .HasMany(p => p.SubTemas);

            modelBuilder.Entity<Tema>()
                .HasMany(p => p.SubTemas);

            modelBuilder.Entity<Produto>()
                .Property(b => b.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<LogProjeto>()
                .Property(b => b.Created)
                .HasDefaultValueSql("getdate()");

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
            builder.Entity<Fornecedor>().ToTable("Fornecedores");
            builder.Entity<Contrato>().ToTable("Contratos");
            builder.Entity<Clausula>().ToTable("Clausulas");
            builder.Entity<CoExecutor>().ToTable("CoExecutores");

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

            builder.Entity<PropostaFornecedor>(builder =>
            {
                builder.HasIndex(p => new {p.CaptacaoId, p.FornecedorId}).IsUnique();
            });

            builder.Entity<CaptacaoInfo>().ToView("CaptacoesView");
        }
    }
}