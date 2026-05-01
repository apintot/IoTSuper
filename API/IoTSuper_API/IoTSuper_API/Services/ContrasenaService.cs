using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace IoTSuper_API.Services
{
    public class ContrasenaService : IContrasenaService
    {
        private readonly PasswordHasher<object> _hasher = new();

        public bool EsContrasenaSegura(string Contrasena)
        {
            if (Contrasena.Length < 12) return false;
            if (!Contrasena.Any(char.IsUpper)) return false;
            if (!Contrasena.Any(char.IsLower)) return false;
            if (!Contrasena.Any(char.IsDigit)) return false;
            if (!Contrasena.Any(ch => !char.IsLetterOrDigit(ch))) return false;

            return true;
        }

        public string hashContrasena(string contrasena)
        {
            return _hasher.HashPassword(new object(), contrasena);
        }

        public bool VerificarContrasena(string hash, string contrasena)
        {
            PasswordVerificationResult result = _hasher.VerifyHashedPassword(new object(), hash, contrasena);
            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
