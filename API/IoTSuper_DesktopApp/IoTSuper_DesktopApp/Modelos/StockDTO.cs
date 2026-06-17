namespace IoTSuper_DesktopApp.Modelos
{
    public class StockDTO
    {
        public int IdStock { get; set; }

        public int IdComponente { get; set; }

        public double Stock_Actual { get; set; }

        public double Stock_Maximo { get; set; }

        public double Stock_Minimo { get; set; }

        public string EmailEmergencia { get; set; } = string.Empty;

        public string TelefonoEmergencia { get; set; } = string.Empty;
    }
}