using Microsoft.EntityFrameworkCore;
using CitasMedicas.Models;

namespace CitasMedicas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> CitasAgregadas { get; set; }
        public DbSet<Cita> CitasCanceladas { get; set; }
    }
}