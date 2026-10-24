namespace CitasMedicas.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
    }

    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
    }

    public class Cita
    {
        public int Id { get; set; }
        public string PacienteNombre { get; set; } = string.Empty;
        public string MedicoNombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}