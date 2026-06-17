using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    public class Evento
    {
        [Key]
        [Column("id_evento")]
        public int IdEvento { get; set; }

        [Required]
        [Column("id_componente")]
        public int IdComponente { get; set; }

        [Required]
        [Column("tipo_evento")]
        [Length(100, 5)]
        public string TipoEvento { get; set; }

        [Required]
        [Column("fecha_evento")]
        public DateTime FechaEvento { get; set; } = DateTime.Now;
    }
}
