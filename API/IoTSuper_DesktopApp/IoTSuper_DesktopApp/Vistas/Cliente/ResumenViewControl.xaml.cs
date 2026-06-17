using IoTSuper_DesktopApp.Config;
using System;
using System.Collections.Generic;
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

namespace IoTSuper_DesktopApp.Vistas.Cliente
{
    /// <summary>
    /// Lógica de interacción para ResumenViewControl.xaml
    /// </summary>
    public partial class ResumenViewControl : UserControl
    {
        public ResumenViewControl()
        {
            InitializeComponent();
            this.Loaded += ResumenViewControl_Loaded;

            Sesion.OnComponenteActualizado -= Sesion_OnComponenteActualizado;
            Sesion.OnComponenteActualizado += Sesion_OnComponenteActualizado;
        }

        private void Sesion_OnComponenteActualizado()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                txbOk.Text = Sesion.Componentes.Count(c => c.Estado == "OK").ToString();
                txbEnError.Text = Sesion.Componentes.Count(c => c.Estado == "Error").ToString();
                txbAlerta.Text = Sesion.Componentes.Count(c => c.Estado == "Alerta!").ToString();
                txbMinimo.Text = Sesion.Componentes.Count(c => c.Estado == "Agotandose").ToString();
            });
        }

        private void ResumenViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgComponentes.ItemsSource = Sesion.Componentes;
            txbEnError.Text = Sesion.Componentes.Count.ToString();
        }
    }
}
