using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Modelos
{
    public class ActualizarClienteResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; } = 200;
        public Dictionary<string, List<string>> Errors { get; set; } = new();
        public string TraceId { get; set; } = string.Empty;
    }
}
