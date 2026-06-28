using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.DTO.Componentes
{
    public class StockDTO
    {
        public int IdStock { get; set; }

        public int IdComponente { get; set; }

        public double Stock_Actual { get; set; }

        public int Stock_Maximo { get; set; }

        public int Stock_Minimo { get; set; }

        public double Peso_Unidad { get; set; }

        public string EmailEmergencia { get; set; } = string.Empty;

        public string TelefonoEmergencia { get; set; } = string.Empty;
    }
}