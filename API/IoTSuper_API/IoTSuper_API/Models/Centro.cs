using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    public class Centro
    {
        [Key]
        [Column("id_centro")]
        public int IdCentro { get; set; }

        [Required]
        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Required]
        [Column("id_tipologia")]
        public int IdTipologia { get; set; }

        [Required]
        [Column("id_localizacion")]
        public int IdLocalizacion { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required]
        [Column("habilitado")]
        public bool Habilitado { get; set; } = true;

        [Required]
        [StringLength(255, MinimumLength = 0)]
        [Column("imagen")]
        public string Imagen { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        [Column("cif")]
        public string Cif { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [Column("razon_social")]
        public string RazonSocial { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = new DateTime(1900, 1, 1);
    }
}
