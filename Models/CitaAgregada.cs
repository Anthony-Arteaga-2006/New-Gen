using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models;

[Table("citasagregadas")]
public class CitaAgregada : Cita
{
    // Hereda automáticamente Id, PacienteId, Paciente, MedicoId, Medico, FechaHora, Motivo y Estado desde Cita.
}