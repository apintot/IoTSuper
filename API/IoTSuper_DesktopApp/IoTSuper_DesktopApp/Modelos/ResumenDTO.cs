using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

namespace IoTSuper_DesktopApp.Modelos
{
    public class ResumenDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public int IdComponente { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Centro { get; set; }
        public string Seccion { get; set; }

        private string _actualizado { get; set; }
        public string Actualizado
        {
            get => _actualizado;
            set { _actualizado = value; OnPropertyChanged(); }
        }

        private string _ultimoDato = "N/A";
        public string UltimoDato
        {
            get => _ultimoDato;
            set { _ultimoDato = value; OnPropertyChanged(); }
        }

        private string _estado = "Error";
        public string Estado
        {
            get => _estado;
            set { _estado = value; OnPropertyChanged(); }
        }

        private SolidColorBrush _estadoColor = new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44));
        public SolidColorBrush EstadoColor
        {
            get => _estadoColor;
            set { _estadoColor = value; OnPropertyChanged(); }
        }

    }
}
