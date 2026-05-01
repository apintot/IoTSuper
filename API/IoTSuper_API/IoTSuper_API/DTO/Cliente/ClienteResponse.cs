namespace IoTSuper_API.DTO.Cliente
{
    public class ClienteResponse
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public bool Habilitado { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public DateTime UltimoAcceso { get; set; }
    }
}
