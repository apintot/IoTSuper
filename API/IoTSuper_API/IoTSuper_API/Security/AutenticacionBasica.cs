namespace IoTSuper_API.Security
{
    public class AutenticacionBasica
    {
        public const string SectionName = "ApiBasicAuth";

        public string AutentificacionBasicaUsuario { get; set; } = string.Empty;
        public string AutentificacionBasicaContrasena { get; set; } = string.Empty;
    }
}
