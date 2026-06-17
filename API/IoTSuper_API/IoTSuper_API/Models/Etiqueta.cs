using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    public class Etiqueta
    {
        [Key]
        [Column("id_etiqueta")]
        public int IdEtiqueta { get; set; }

        [Column("id_componente")]
        public int IdComponente { get; set; }

        [Required]
        [Length(20, 5)]
        public string Frase1 { get; set; }

        [Required]
        [Length(20, 5)]
        public string Frase2 { get; set; }

        [Length(20, 5)]
        public string Frase3 { get; set; } = string.Empty;

        [Length(20, 5)]
        public string Frase4 { get; set; } = string.Empty;

        public int Visualizaciones { get; set; } = 0;
    }
}
