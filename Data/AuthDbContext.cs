using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        // Tablas
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RolUsuario> RolUsuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar la relación Usuario -> RolUsuario
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)                  // Propiedad de navegación
                .WithMany()                           // No hay colección inversa
                .HasForeignKey(u => u.IdRolUsuario)  // FK explícita
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar propiedades de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(u => u.Nombre).IsRequired();
                entity.Property(u => u.UserName).IsRequired();
                entity.Property(u => u.Contrasena).IsRequired();
                entity.Property(u => u.FechaCreacion)
                      .HasDefaultValueSql("GETDATE()");
                entity.Property(u => u.Activo)
                      .HasDefaultValue(true);
            });

            // Configurar propiedades de RolUsuario
            modelBuilder.Entity<RolUsuario>(entity =>
            {
                entity.Property(r => r.Nombre).IsRequired();
                entity.Property(r => r.FechaCreacion)
                      .HasDefaultValueSql("GETDATE()");
                entity.Property(r => r.Activo)
                      .HasDefaultValue(true);
            });
        }
    }
}
