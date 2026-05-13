
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.DTO.Tipologia
{
    public class TipologiaDTO
    {
        [StringLength(50, MinimumLength = 5)]
        public string TipoTienda { get; set; }
    }
}
