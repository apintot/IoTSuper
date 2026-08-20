using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.RClone;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IoTSuper_DesktopApp.RClone
{
    public static class RClone
    {
        public static async Task<bool> SubirImagenesAlServidorAsync()
        {
            LogLocal.logear($"Subiendo imágenes al servidor...");

            string subida = $"sync \"C:\\Users\\Pc\\AppData\\Roaming\\IoTSuper\\Imagenes\" \":sftp,host={RCloneConfig.dominio},user={RCloneConfig.usuario},pass={RCloneConfig.contrasena}:/home/iotsuper/imagenes/{Sesion.LoginData.IdCliente}\" --progress";

            return await ejecutarComandoRclone(subida);
        }

        public static async Task<bool> BajarImagenesDelServidorAsync()
        {
            LogLocal.logear($"Bajando imágenes del servidor...");

            string bajada = $"sync \":sftp,host={RCloneConfig.dominio},user={RCloneConfig.usuario},pass={RCloneConfig.contrasena}:/home/iotsuper/imagenes/{Sesion.LoginData.IdCliente}\" \"C:\\Users\\Pc\\AppData\\Roaming\\IoTSuper\\Imagenes\" --progress";

            return await ejecutarComandoRclone(bajada);
        }

        private static async Task<bool> ejecutarComandoRclone(string accion)
        {
            LogLocal.logear($"Ejecutando comando RClone: {accion}");

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Process proceso = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = $"{Rutas.RClone}\\rclone",
                    Arguments = accion,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            proceso.Exited += (s, e) =>
            {
                tcs.SetResult(proceso.ExitCode == 0);
                proceso.Dispose();
            };

            proceso.Start();
            return await tcs.Task;
        }
    }
}
