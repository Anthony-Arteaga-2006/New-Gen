using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models
{
    public class Paciente
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string CI { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}