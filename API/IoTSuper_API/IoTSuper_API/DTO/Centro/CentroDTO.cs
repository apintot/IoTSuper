using IoTSuper_API.DTO.Localizacion;
using IoTSuper_API.DTO.Tipologia;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.DTO.Centro
{
    public class CentroDTO
    {
        public int IdCentro { get; set; }
        public int IdCliente { get; set; }
        public int IdTipologia { get; set; }
        public int IdLocalizacion { get; set; }

        public bool Habilitado { get; set; } = true;

        [StringLength(150, MinimumLength = 5)]
        public string Nombre { get; set; }

        public string Imagen { get; set; }

        [StringLength(20, MinimumLength = 5)]
        public string Cif { get; set; }

        [StringLength(20, MinimumLength = 5)]
        public string RazonSocial { get; set; }
            

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public LocalizacionDTO Localizacion { get; set; } = null;
        public TipologiaDTO Tipologia { get; set; } = null;
    }
}
