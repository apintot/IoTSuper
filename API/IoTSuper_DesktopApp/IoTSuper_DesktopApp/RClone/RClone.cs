using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.RClone;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace IoTSuper_DesktopApp.RClone
{
    public static class RClone
    {
        private static readonly string rutaLocal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IoTSuper", "Imagenes");
        
        public static async Task<bool> SubirImagenesAlServidorAsync()
        {
            LogLocal.logear($"Subiendo imágenes al servidor...");

            string subida = $"sync \"{rutaLocal}\" \":sftp,host={RCloneConfig.dominio},user={RCloneConfig.usuario},pass={RCloneConfig.contrasena}:/home/iotsuper/imagenes/{Sesion.LoginData.IdCliente}\" --progress";

            return await ejecutarComandoRclone(subida);
        }

        public static async Task<bool> BajarImagenesDelServidorAsync()
        {
            LogLocal.logear($"Bajando imágenes del servidor...");

            Directory.CreateDirectory(rutaLocal);

            string bajada = $"sync \":sftp,host={RCloneConfig.dominio},user={RCloneConfig.usuario},pass={RCloneConfig.contrasena}:/home/iotsuper/imagenes/{Sesion.LoginData.IdCliente}\" \"{rutaLocal}\" --progress";

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
