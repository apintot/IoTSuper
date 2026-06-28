using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    public class Stock
    {
        [Key]
        [Column("id_stock")]
        public int IdStock { get; set; }

        [Column("id_componente")]
        public int IdComponente { get; set; }

        [Column("stock_maximo")]
        public int Stock_Maximo { get; set; }

        [Column("stock_minimo")]
        public int Stock_Minimo { get; set; }

        [Column("peso_unidad")]
        public double Peso_Unidad { get; set; }

        [Column("email_emergencia")]
        [Length(100, 20)]
        public string EmailEmergencia { get; set; }

    }
}
