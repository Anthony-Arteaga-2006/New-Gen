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
        public DbSet<CitaAgregada> CitasAgregadas { get; set; }
        public DbSet<CitaCancelada> CitasCanceladas { get; set; }

        public DbSet<Historial> Historial { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración explícita de herencia TPT (Tabla por Tipo) en MariaDB
            modelBuilder.Entity<Cita>().ToTable("citas");
            modelBuilder.Entity<CitaAgregada>().ToTable("citasagregadas");
            modelBuilder.Entity<CitaCancelada>().ToTable("citascanceladas");
            modelBuilder.Entity<Historial>().ToTable("Historial");
        }
    }
}