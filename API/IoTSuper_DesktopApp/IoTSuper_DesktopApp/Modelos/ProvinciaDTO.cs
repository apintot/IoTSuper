using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace IoTSuper_DesktopApp.Modelos
{
    public class ProvinciaDTO
    {
        public bool error { get; set; }
        public string msg { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string name { get; set; }
        public string iso3 { get; set; }
        public string iso2 { get; set; }
        public List<State> states { get; set; }
    }

    public class State
    {
        public string name { get; set; }
        public string state_code { get; set; }
    }
}

