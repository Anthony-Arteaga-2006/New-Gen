using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models
{
    [Table("citascanceladas")]
    public class CitaCancelada
    {
        public int Id { get; set; }

        [Required]
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        [Required]
        public int MedicoId { get; set; }
        public Medico? Medico { get; set; }

        public DateTime FechaHoraOriginal { get; set; }
        public DateTime FechaCancelacion { get; set; } = DateTime.Now;
        public string? MotivoCancelacion { get; set; }
    }
}