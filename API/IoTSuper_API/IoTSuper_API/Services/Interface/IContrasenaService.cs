namespace IoTSuper_API.Services.Interface
{
    public interface IContrasenaService
    {
        public bool EsContrasenaSegura(string Contrasena);
        public string hashContrasena(string contrasena);
        public bool VerificarContrasena(string contrasena, string hash);
    }
}
