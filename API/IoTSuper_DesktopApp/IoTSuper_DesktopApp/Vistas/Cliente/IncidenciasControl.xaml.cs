using IoTSuper_DesktopApp.Servicios.Eventos;
using System;
using System.Collections.Generic;
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

namespace IoTSuper_DesktopApp.Vistas.Cliente
{
    /// <summary>
    /// Lógica de interacción para IncidenciasControl.xaml
    /// </summary>
    public partial class IncidenciasControl : UserControl
    {
        public IncidenciasControl()
        {
            InitializeComponent();

            this.Loaded += IncidenciasControl_Loaded;
        }

        private async void IncidenciasControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgEventos.ItemsSource = await EventosServices.obtenerEventosRecientes();
        }
    }
}
