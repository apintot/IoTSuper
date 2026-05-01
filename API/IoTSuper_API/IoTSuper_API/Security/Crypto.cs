using Microsoft.AspNetCore.DataProtection;
using System.Text;

namespace IoTSuper_API.Security
{
    public class Crypto
    {
        private Seguridad seguridad;

        public const string SectionName = "Encriptacion";

        public string claveEncriptacion { get; set; }
        public string vectorEncriptacion { get; set; }

        public Crypto(string vectorEncriptacion, string claveEncriptacion)
        {
            this.vectorEncriptacion = vectorEncriptacion;
            this.claveEncriptacion = claveEncriptacion;
            seguridad = new Seguridad(this.vectorEncriptacion, this.claveEncriptacion);
        }

        public string Encriptar(string texto)
        {
            string _texto = Convert.ToBase64String(Encoding.UTF8.GetBytes(texto));
            return seguridad.protector.Protect(_texto);
        }

        public string Desencriptar(string texto)
        {
            string _texto = seguridad.protector.Unprotect(texto);
            return Encoding.UTF8.GetString(Convert.FromBase64String(_texto));
        }
    }

    internal class Seguridad
    {
        private IDataProtectionProvider provider;

        public IDataProtector protector;

        public Seguridad(string _provider, string _protector)
        {
            provider = DataProtectionProvider.Create(_provider);
            protector = provider.CreateProtector(_protector);
        }
    }
}
