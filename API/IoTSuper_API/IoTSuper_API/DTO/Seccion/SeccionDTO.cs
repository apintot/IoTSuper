using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.DTO.Seccion
{
    public class SeccionDTO
    {
        public int IdSeccion { get; set; }
        public int IdCentro { get; set; }

        [StringLength(150, MinimumLength = 5)]
        public string Nombre { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;

        public bool Habilitado { get; set; } = true;

        public DateTime? UpdateAt { get; set; } = new DateTime(1900, 1, 1);
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
