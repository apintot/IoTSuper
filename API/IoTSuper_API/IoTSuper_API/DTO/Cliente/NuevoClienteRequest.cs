using System.ComponentModel.DataAnnotations;

namespace IoTSuper_API.DTO.Cliente
{
    public class NuevoClienteRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Empresa { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 12)]
        public string Contrasena { get; set; } = string.Empty;
    }
}
