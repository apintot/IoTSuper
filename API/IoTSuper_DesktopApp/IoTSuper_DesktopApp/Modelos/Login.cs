using IoTSuper_DesktopApp.Seguridad;

namespace IoTSuper_DesktopApp.Modelos
{
    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string contrasena { get; set; }
        public string TOTP { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public int IdCliente { get; set; }
        public bool EsAdmin { get; set; }
        public string TOTP { get; set; } = string.Empty;
        public DateTime ultimoAcceso { get; set; }
    }

    public class ApiConfigFolder
    {
        internal Crypto Crypto = new Crypto();
        public string APIUsuario { get; set; }
        public string APIcontrasena { get; set; }
        public string API { get; set; }
        public string EndPointLogin { get; set; }
        public string EndPointCliente { get; set; }
        public string EndPointActualizarTOTP { get; set; }

        public ApiConfigFolder()
        {
            APIUsuario = Crypto.Encriptar("IoTSuperUser");
            APIcontrasena = Crypto.Encriptar("RjP&y3WT6gbH0$!7R#8w");
            API = "http://localhost:5188";
            EndPointLogin = "/IoTSuper/Login";
            EndPointActualizarTOTP = "/IoTSuper/ActualizarTOTP";
            EndPointCliente = "/IoTSuper/Clientes";
        }
    }
}