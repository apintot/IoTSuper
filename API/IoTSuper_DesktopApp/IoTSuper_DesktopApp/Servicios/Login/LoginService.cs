using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Servicios
{
    public static class LoginService
    {
        public static async Task<LoginResponse> IniciarSesionAsync(string usuario, string contrasena)
        {
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
            {
                throw new ArgumentException("El usuario y la contraseña no pueden estar vacíos.");
            }

            LoginRequest request = new LoginRequest() { Usuario = usuario, contrasena = contrasena };

            return await APIService.PostAsync<LoginResponse>(Sesion.ApiConfigFolder.EndPointLogin, request) ?? new LoginResponse();
        }

        public static async Task<bool> ActualizarTOTP(string totp)
        {
            if (string.IsNullOrEmpty(totp))
            {
                throw new ArgumentException("El TOTP esta vacio");
            }

            return await APIService.PutAsync<bool>(Sesion.ApiConfigFolder.EndPointLogin + $"/{Sesion.LoginData.IdCliente}", new { Totp = totp });
        }
    }
}
