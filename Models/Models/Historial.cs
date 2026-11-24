using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models
{
    [Table("Historial")]
    public class Historial
    {
        [Key]
        public int Id { get; set; }

        public int CitaId { get; set; }
        [ForeignKey("CitaId")]
        public Cita? Cita { get; set; }

        [Required]
        public string Accion { get; set; } = string.Empty;

        [Required]
        public string Detalle { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}