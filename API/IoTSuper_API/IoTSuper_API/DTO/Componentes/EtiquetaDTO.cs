using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.DTO.Componentes
{
    public class EtiquetaDTO
    {
        public int IdEtiqueta { get; set; }

        public int IdComponente { get; set; }

        public string Frase1 { get; set; }

        public string Frase2 { get; set; } = string.Empty;

        public string Frase3 { get; set; } = string.Empty;

        public string Frase4 { get; set; } = string.Empty;

        public int Visualizaciones { get; set; } = 0;
    }
}