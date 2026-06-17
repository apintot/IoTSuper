using IoTSuper_DesktopApp.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IoTSuper_DesktopApp.Helpers
{
    public static class LogLocal
    {
        public static void logear(string texto)
        {
            if (!Directory.Exists(Rutas.Logs))
            {
                Directory.CreateDirectory(Rutas.Logs);
            }

            string nombreLog = "LogLocal_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-') + ".log";

            File.AppendAllText(Path.Combine(Rutas.Logs, nombreLog), "\n" + texto);
        }
    }
}
