using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.DTO.Componentes
{
    public class TermometroDTO
    {
        public int IdTermometro { get; set; }

        public int IdComponente { get; set; }

        public double Temperatura_Maxima { get; set; }

        public double Temperatura_Minima { get; set; }

        public string EmailEmergencia { get; set; }

        public string TelefonoEmergencia { get; set; }
    }
}