using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    public class Localizacion
    {
        [Key]
        [Column("id_localizacion")]
        public int IdLocalizacion { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("direccion")]
        public string Direccion { get; set; }

        [Required]
        [Column("codigo_postal")]
        public string CodigoPostal { get; set; }

        [Required]
        [MaxLength(80)]
        [Column("pais")]
        public string Pais { get; set; }

        [Required]
        [MaxLength(80)]
        [Column("Provincia")]
        public string provincia { get; set; }
    }
}
