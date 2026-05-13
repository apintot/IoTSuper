using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    public class Tipologia
    {
        [Key]
        [Column("id_tipologia")]
        public int IdTipologia { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("tipo_tienda")]
        public string TipoTienda { get; set; }
    }
}
