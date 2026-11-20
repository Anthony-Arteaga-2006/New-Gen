using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models;

[Table("citascanceladas")]
public class CitaCancelada : Cita
{
    public DateTime FechaHoraOriginal { get; set; }
    public DateTime FechaCancelacion { get; set; } = DateTime.Now;
    public string? MotivoCancelacion { get; set; }
}