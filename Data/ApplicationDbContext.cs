using Microsoft.EntityFrameworkCore;
using CitasMedicas.Models;

namespace CitasMedicas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Historial> Historial { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cita>(entity =>
            {
                entity.ToTable("citas");
                entity.Property(c => c.Estado)
                      .HasConversion<string>()
                      .HasDefaultValue(EstadoCita.Activa);
            });

            modelBuilder.Entity<Historial>().ToTable("Historial");
        }
    }
}