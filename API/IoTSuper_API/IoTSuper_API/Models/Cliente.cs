using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTSuper_API.Models
{
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5)]
        [Column("apellido")]
        public string Apellido { get; set; }

        [Required]
        [Column("habilitado")]
        public bool Habilitado { get; set; } = true;

        [Required]
        [Column("esAdmin")]
        public bool EsAdmin { get; set; } = false;

        [StringLength(150, MinimumLength = 5)]
        [Column("empresa")]
        public string Empresa { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        [Column("login")]
        public string Login { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [Column("contraseña")]
        public string Contrasena { get; set; }

        [StringLength(255, MinimumLength = 5)]
        [Column("totp")]
        public string Totp { get; set; } = string.Empty;

        [Column("ultimo_acceso")]
        public DateTime? UltimoAcceso { get; set; } = new DateTime(1900, 1, 1);

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
