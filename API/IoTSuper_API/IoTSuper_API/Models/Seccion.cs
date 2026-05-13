using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    public class Seccion
    {
        [Key]
        [Column("id_seccion")]
        public int IdSeccion { get; set; }

        [Required]
        [Column("id_centro")]
        public int IdCentro { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("imagen")]
        public string Imagen { get; set; }

        [Required]
        [Column("habilitado")]
        public bool Habilitado { get; set; } = true;

        [Column("update_at")]
        public DateTime? UpdateAt { get; set; } = new DateTime(1900, 1, 1);

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
