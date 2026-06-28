using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Config
{
    public static class Rutas
    {
        public static readonly string AppFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IoTSuper");
        public static readonly string ApiConfigFile = System.IO.Path.Combine(AppFolder, "api_config.json");
        public static readonly string ImagesFolder = System.IO.Path.Combine(AppFolder, "Imagenes");
        public static readonly string Logs = System.IO.Path.Combine(AppFolder, "Logs");

        public static readonly string RClone = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rclone");
    }
}
