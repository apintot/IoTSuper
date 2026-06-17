using IoTSuper_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.DTO.Componentes
{
    public class ComponenteDTO
    {
        public int IdComponente { get; set; }

        public int IdSeccion { get; set; }

        public string Nombre { get; set; }

        public string Topic { get; set; }

        public double PosicionX { get; set; }

        public double PosicionY { get; set; }

        public TermometroDTO? Termometro { get; set; }

        public StockDTO? Stock { get; set; } 

        public EtiquetaDTO? Etiqueta { get; set; }
    }
}
