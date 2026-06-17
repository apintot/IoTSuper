using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    [Index(nameof(Topic), IsUnique = true)]
    public class Componente
    {
        [Key]
        [Column("id_componente")]
        public int IdComponente { get; set; }

        [Required]
        [Column("id_seccion")]
        public int IdSeccion { get; set; }

        [Required]
        [Length(150, 5)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required]
        [Length(150, 10)]
        [Column("topic")]
        public string Topic { get; set; }

        [Required]
        [Column("habilitado")]
        public bool Habilitado { get; set; } = true;

        [Required]
        [Column("posX")]
        public double PosicionX { get; set; }

        [Required]
        [Column("posY")]
        public double PosicionY { get; set; }

        [Column("update_at")]
        public DateTime? UpdateAt { get; set; } = new DateTime(1900, 1, 1);

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
