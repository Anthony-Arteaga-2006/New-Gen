using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models
{
    [Table("citasagregadas")]
    public class CitaAgregada
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

        public string? Motivo { get; set; }
        public string Estado { get; set; } = "Agendada";
    }
}