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
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [Column("apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [Column("habilitado")]
        public bool Habilitado { get; set; } = true;

        [Required]
        [Column("esAdmin")]
        public bool EsAdmin { get; set; } = false;

        [MaxLength(150)]
        [Column("empresa")]
        public string Empresa { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("login")]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("contraseña")]
        public string Contrasena { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("totp")]
        public string Totp { get; set; } = string.Empty;

        [Column("ultimo_acceso")]
        public DateTime? UltimoAcceso { get; set; } = new DateTime(1900, 1, 1);

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
