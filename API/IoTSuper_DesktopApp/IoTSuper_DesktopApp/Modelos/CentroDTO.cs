using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IoTSuper_DesktopApp.Modelos
{
    public class CentroDTO
    {
        public int IdCentro { get; set; } = 0;
        public int IdCliente { get; set; }
        public int IdTipologia { get; set; }
        public int IdLocalizacion { get; set; }

        public bool Habilitado { get; set; } = true;

        public string Nombre { get; set; }

        public string Imagen { get; set; }

        public string Cif { get; set; }

        public string RazonSocial { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public LocalizacionDTO Localizacion { get; set; } = new LocalizacionDTO();
    }
}
