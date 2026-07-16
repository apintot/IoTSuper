using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Modelos
{
    public class PaisesDTO
    {
        public bool error { get; set; }
        public string msg { get; set; }
        public InfoPaises[] data { get; set; }

        public class InfoPaises
        {
            public string iso2 { get; set; }
            public string iso3 { get; set; }
            public string country { get; set; }
            public string[] cities { get; set; }
        }
    }
}


