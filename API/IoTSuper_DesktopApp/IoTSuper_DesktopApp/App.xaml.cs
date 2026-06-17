using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace IoTSuper_DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CrearCarpetasyFicheros();
        }

        private void CrearCarpetasyFicheros()
        {
            if (!Directory.Exists(Rutas.AppFolder))
            {
                Directory.CreateDirectory(Rutas.AppFolder);
            }

            if (!Directory.Exists(Rutas.Logs))
            {
                Directory.CreateDirectory(Rutas.Logs);
            }

            if (!File.Exists(Rutas.ApiConfigFile))
            {
                FileWriter.Write<ApiConfigFolder>(Rutas.ApiConfigFile, new ApiConfigFolder());
            }

            Sesion.ApiConfigFolder = JsonSerializer.Deserialize<ApiConfigFolder>(File.ReadAllText(Rutas.ApiConfigFile)) ?? new ApiConfigFolder();
        }
    }
}
