using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models
{
    public class Medico
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string CI { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Especialidad { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}