using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Modelos
{
    public class PaisesDTO
    {
        public Name name { get; set; }

        public class Name
        {
            public string common { get; set; }
            public string official { get; set; }
        }
    }
}
