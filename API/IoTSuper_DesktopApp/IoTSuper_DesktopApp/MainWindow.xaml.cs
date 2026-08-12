using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Centro;
using IoTSuper_DesktopApp.Servicios.Componente;
using IoTSuper_DesktopApp.Vistas.Administrador;
using IoTSuper_DesktopApp.Vistas.Cliente;
using System.ComponentModel;
using System.Diagnostics;
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
                ocultarVistas();
            }
            else
            {
                this.Loaded += MainWindow_Loaded;
                this.IsHitTestVisible = false;
            }
        }

        private void ocultarVistas()
        {
            foreach(UIElement elemento in this.VistasGrid.Children)
            {
                if (elemento is not StackPanel stackPanel) continue;
                foreach (StackPanel hijo in stackPanel.Children)
                {
                    if(hijo.Tag?.ToString() == "1")
                    {
                        hijo.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bool exito = await RClone.RClone.BajarImagenesDelServidorAsync();

            Sesion._centros = await CentroService.ObtenerCentros(Sesion.LoginData.IdCliente);

            foreach(CentroDTO centro in Sesion._centros)
            {
                centro._secciones = await CentroService.ObtenerSeccionesCentro(centro.IdCentro);
                
                foreach(SeccionDTO seccion in centro._secciones)
                {
                    seccion._componentes = await ComponenteService.ObtenerComponentesSeccion(seccion.IdSeccion);
                }
            }

            cargarTablaResumen();

            Sesion.conectarAMqtt();

            Navegacion.IrA(new ResumenViewControl());
            Sesion._stopwatch.Stop();
            Debug.WriteLine("Programa cargado en: " + Sesion._stopwatch.ElapsedMilliseconds + " ms");
            this.IsHitTestVisible = true;
        }

        private void cargarTablaResumen()
        {
            List<ResumenDTO> resumenDTOs = Sesion._centros.SelectMany(c => c._secciones.SelectMany(s => s._componentes.Select(x => ComponenteToResumen.ConvierteAResumenDTO(x, c.Nombre, s.Nombre)))).ToList();

            foreach(ResumenDTO resumenDTO in resumenDTOs)
            {
                Sesion.Componentes.Add(resumenDTO);
            }
        }

        private void navegar(UserControl vista)
        {
            VistaActual = vista;
        }

        private void VerCentros_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(VistaActual is not CarruselCentro)
                Navegacion.IrA(new CarruselCentro());
        }

        private void CrearCentro_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(VistaActual is not FormularioCentroControl)
                Navegacion.IrA(new FormularioCentroControl());
        }

        private void Inicio_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Sesion.LoginData.EsAdmin)
            {
                Navegacion.IrA(new AdminInicio());
            }
            else
            {
                Navegacion.IrA(new ResumenViewControl());
            }
        }

        private void Incidencias_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Navegacion.IrA(new IncidenciasControl());
        }
    }
}