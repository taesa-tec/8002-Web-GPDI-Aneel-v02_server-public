using Microsoft.EntityFrameworkCore;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace APIGestor.Data
{
    public class GestorDbContext : IdentityDbContext<ApplicationUser>
    {
        public GestorDbContext(
            DbContextOptions<GestorDbContext> options) : base(options)
        { }
        public DbSet<LogProjeto> LogProjetos { get; set; }
        public DbSet<CatalogUserPermissao> CatalogUserPermissoes { get; set; }
        public DbSet<CatalogStatus> CatalogStatus { get; set; }
        public DbSet<CatalogSegmento> CatalogSegmentos { get; set; }
        public DbSet<CatalogEmpresa> CatalogEmpresas { get; set; }
        public DbSet<CatalogEstado> CatalogEstados { get; set; }
        public DbSet<CatalogTema> CatalogTema { get; set; }
        public DbSet<CatalogSubTema> CatalogSubTemas { get; set; }
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
        public DbSet<RecursoMaterial> RecursoMaterais { get; set; }
        public DbSet<AlocacaoRm> AlocacoesRm { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.Tema);
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

            base.OnModelCreating(modelBuilder);
        }
    }
}