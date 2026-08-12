namespace IoTSuper_DesktopApp.Modelos
{
    public class TermometroDTO
    {
        public int IdTermometro { get; set; }

        public int IdComponente { get; set; }

        public double Temperatura_Actual { get; set; }

        public double Temperatura_Maxima { get; set; }

        public double Temperatura_Minima { get; set; }

        public string EmailEmergencia { get; set; }
    }
}