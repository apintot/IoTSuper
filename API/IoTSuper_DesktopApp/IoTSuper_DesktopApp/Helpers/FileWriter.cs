using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Seguridad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace IoTSuper_DesktopApp.Helpers
{
    public static class FileWriter
    {
        public static void Write<T>(string path, T content) 
        {
            File.WriteAllText(path, JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
