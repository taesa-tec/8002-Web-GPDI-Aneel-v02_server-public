using Microsoft.EntityFrameworkCore;
using APIGestor.Models;

namespace APIGestor.Data
{
    public class GestorDbContext : DbContext
    {
        public GestorDbContext(
            DbContextOptions<GestorDbContext> options) : base(options)
        { }

        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projeto>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Projeto>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()");
            
            modelBuilder.Entity<Projeto>()
            .HasMany(p => p.Produtos)
            .WithOne(b => b.Projeto);

            modelBuilder.Entity<Produto>()
            .HasOne(p => p.Projeto)
            .WithMany(b => b.Produtos);

            modelBuilder.Entity<Empresa>()
                .HasKey(p => p.Id);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}