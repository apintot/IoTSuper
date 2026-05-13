namespace IoTSuper_API.DTO.Login
{
    public class LoginResponse
    {
        public int IdCliente { get; set; }
        public bool EsAdmin { get; set; }
        public string TOTP { get; set; } = string.Empty;
        public DateTime ultimoAcceso { get; set; }
    }
}
