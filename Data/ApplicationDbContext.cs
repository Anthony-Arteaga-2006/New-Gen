using Microsoft.EntityFrameworkCore;
using CitasMedicas.Models;

namespace CitasMedicas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<CitaAgregada> CitasAgregadas { get; set; }
        public DbSet<CitaCancelada> CitasCanceladas { get; set; }
    }
}