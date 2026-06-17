using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IoTSuper_DesktopApp.Modelos
{
    public class SeccionDTO
    {
        public int IdSeccion { get; set; } = 0;
        public int IdCentro { get; set; } = 0;

        public string Nombre { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;

        public bool Habilitado { get; set; } = true;

        public int NumComponentes = 0;

        public DateTime? UpdateAt { get; set; } = new DateTime(1900, 1, 1);
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<ComponenteDTO> _componentes;
    }
}
