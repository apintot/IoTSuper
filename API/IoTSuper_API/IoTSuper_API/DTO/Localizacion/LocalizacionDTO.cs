using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.DTO.Localizacion
{
    public class LocalizacionDTO
    {
        [StringLength(255, MinimumLength = 5)]
        public string Direccion { get; set; }

        [StringLength(10, MinimumLength = 5)]
        public string CodigoPostal { get; set; }

        [StringLength(80, MinimumLength = 5)]
        public string Pais { get; set; }

        [StringLength(80, MinimumLength = 5)]
        public string provincia { get; set; }
    }
}
