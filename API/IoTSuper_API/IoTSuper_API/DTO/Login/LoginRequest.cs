namespace IoTSuper_API.DTO.Login
{
    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string TOTP { get; set; } = string.Empty;
    }
}
