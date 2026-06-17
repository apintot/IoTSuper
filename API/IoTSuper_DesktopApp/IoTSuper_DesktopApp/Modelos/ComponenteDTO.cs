using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Modelos
{
    public class ComponenteDTO
    {
        public int IdComponente { get; set; }

        public int IdSeccion { get; set; }

        public string Nombre { get; set; }

        public DateTime UltimaActualizacion { get; set; }

        public string Topic { get; set; }

        public double PosicionX { get; set; }

        public double PosicionY { get; set; }

        public TermometroDTO? Termometro { get; set; }

        public StockDTO? Stock { get; set; }

        public EtiquetaDTO? Etiqueta { get; set; }
    }
}
