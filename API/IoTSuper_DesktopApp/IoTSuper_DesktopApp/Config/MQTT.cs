using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Config
{
    public class MQTT
    {
        public string topic { get; set; } = "IoTSuper";
        public string broker { get; set; } = "iotsuper.duckdns.org";
        public string usuario { get; set; } = "iotsuper";
        public string contrasena { get; set; } = "iotsupermqtt";
    }
}
