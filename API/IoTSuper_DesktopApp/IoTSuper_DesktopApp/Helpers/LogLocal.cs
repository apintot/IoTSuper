using IoTSuper_DesktopApp.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IoTSuper_DesktopApp.Helpers
{
    public static class LogLocal
    {
        private static readonly object _lock = new object();

        public static void logear(string texto)
        {
            lock (_lock)
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
}
