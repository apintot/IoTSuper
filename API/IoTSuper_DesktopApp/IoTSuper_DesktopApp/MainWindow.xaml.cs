using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Vistas.Administrador;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IoTSuper_DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private UserControl _vistaActual;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        public UserControl VistaActual
        {
            get => _vistaActual;
            set
            {
                _vistaActual = value;
                OnPropertyChanged(nameof(VistaActual));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            Navegacion.CambiarVista = navegar;

            if (Sesion.LoginData.EsAdmin)
            {
                Navegacion.IrA(new AdminInicio());
            }
            else
            {
               //Navegacion.IrA(new AdminInicio());
            }
        }

        private void navegar(UserControl vista)
        {
            VistaActual = vista;
        }
    }
}