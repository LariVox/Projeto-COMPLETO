using Microsoft. EntityFrameworkCore;
using ProjetoCompleto.Models;

namespace ProjetoCompleto.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.DataCadastro)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}