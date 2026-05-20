using System.ComponentModel.DataAnnotations;

namespace IoTSuper_DesktopApp.Modelos
{
    public class LocalizacionDTO
    {
        public string Direccion { get; set; }

        public string CodigoPostal { get; set; }

        public string Pais { get; set; }

        public string provincia { get; set; }
    }
}