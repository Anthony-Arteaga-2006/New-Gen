using System.ComponentModel.DataAnnotations;

namespace CitasMedicas.Models
{
    public enum EstadoCita
    {
        Activa,
        Cancelada
    }

    public class Cita
    {
        public int Id { get; set; }

        [Required]
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        [Required]
        public int MedicoId { get; set; }
        public Medico? Medico { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        public string Motivo { get; set; } = string.Empty;

        public EstadoCita Estado { get; set; } = EstadoCita.Activa;

        public DateTime? FechaCancelacion { get; set; }
        public string? MotivoCancelacion { get; set; }
    }
}