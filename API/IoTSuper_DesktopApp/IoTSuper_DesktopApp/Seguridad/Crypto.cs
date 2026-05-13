using Microsoft.AspNetCore.DataProtection;
using System.Text;

namespace IoTSuper_DesktopApp.Seguridad
{
    public class Crypto
    {
        public string claveEncriptacion = "MiTFGUniversidadCadiz";
        public string vectorEncriptacion = "UniversidadCadiz";

        private Seguridad seguridad;

        public Crypto()
        {
            seguridad = new Seguridad(vectorEncriptacion, claveEncriptacion);
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
