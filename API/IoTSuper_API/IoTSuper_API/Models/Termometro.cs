using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    public class Termometro
    {
        [Key]
        [Column("id_termometro")]
        public int IdTermometro { get; set; }

        [Column("id_componente")]
        public int IdComponente { get; set; }

        [Column("temperatura_maxima")]
        public double Temperatura_Maxima { get; set; }

        [Column("temperatura_minima")]
        public double Temperatura_Minima { get; set; }

        [Column("email_emergencia")]
        [Length(100, 20)]
        public string EmailEmergencia { get; set; } = string.Empty;

        [Column("telefono_emergencia")]
        [Length(20, 8)]
        public string TelefonoEmergencia { get; set; } = string.Empty;
    }
}
