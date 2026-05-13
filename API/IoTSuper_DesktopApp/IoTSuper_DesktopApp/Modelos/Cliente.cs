using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IoTSuper_DesktopApp.Modelos
{
    public class Cliente
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;


        public int IdCliente { get; set; }
        public bool EsAdmin { get; set; } = false;
        public bool Habilitado { get; set; } = true;
        public DateTime UltimoAcceso { get; set; }
    }
}
