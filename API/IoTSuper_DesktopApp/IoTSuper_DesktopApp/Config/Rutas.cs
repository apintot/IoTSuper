using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Config
{
    public static class Rutas
    {
        public static readonly string AppFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IoTSuper");
        public static readonly string ApiConfigFile = System.IO.Path.Combine(AppFolder, "api_config.json");
    }
}
